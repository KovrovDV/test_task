using System;
using System.Text;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace single_send_mail
{
    /**<summary>Класс для обработки писем</summary>**/
    class TSendMailSrv : IDisposable {
        /**<summary>Информация о письме</summary>**/
        public byte[] arMessage { get; set;}                       
        private IConnection       fpConnection;
        private IModel            fpChanel;
        /**<summary>Инициализация обработки</summary>
         * <param name="_pParams">параметры очереди</param>         
         * <param name="_sError">Возвращемая строка с ошибкой</param>        
        **/
        public bool Init(TQueueParams _pParams){            
            try{
                // Считываем параметры
                var factory = new ConnectionFactory() { 
                    HostName = _pParams.sHostName,  
                    Port = _pParams.iPort, 
                    VirtualHost = _pParams.sVirtualHast, 
                    UserName =_pParams.sUserName, 
                    Password = _pParams.sPassword
                };
                fpConnection = factory.CreateConnection();
                fpChanel = fpConnection.CreateModel();
                // Для простоты  без топиков а прямо Forward queue (а так можно разделять например на почту и telegram)
                fpChanel.QueueDeclare(queue: MSConsts.S_QUEUE_NAME, 
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                // Добавляем потребителя данных из очереди
                EventingBasicConsumer pConsumer = new EventingBasicConsumer(fpChanel) {  };
                pConsumer.Received += async (m, a) => await ProcEmail(m, a);
                fpChanel.BasicConsume(queue: MSConsts.S_QUEUE_NAME, autoAck: false, consumer: pConsumer);
                return true;
            }catch(Exception E){
                // Или любой другой поток лога (база например)
                Console.WriteLine(MSConsts.S_PROC_ERR_STR, E.Message);
                return false;
            }
        }            

        /**<summary>Обработать почту</summary>
          * <param name="_pModel">раздел</param>
          * <param name="_pArgs">Параметры</param>
        **/
        private async Task ProcEmail(object _pModel, BasicDeliverEventArgs _pArgs) {
            try{
                string sEmail = Encoding.UTF8.GetString(_pArgs.Body.ToArray());
                // Отправка письма и оповещение очереди
                NotiftyQueue(_pArgs.DeliveryTag, await TSendMeth.SendMeth(sEmail, arMessage));
            } catch (Exception E){
                NotiftyQueue(_pArgs.DeliveryTag, false);
                // Или любой другой поток лога (база например)
                Console.WriteLine(MSConsts.S_PROC_ERR_STR, E.Message);
            }
        }
        /**<summary>Оповещаем очередь о результатах обработки</summary>
         * <param name="_iDeliveryTag">поле дедупликации</param>
         * <param name="_fOk">успех</param>
        **/
        private void NotiftyQueue(ulong _iDeliveryTag, bool _fOk) {
            try{
                // Исходя из рекомендаций RabbitMQ pChanel потокобезопасный
                if(_fOk)
                    fpChanel.BasicAck(_iDeliveryTag, false);
                else
                    fpChanel.BasicReject(_iDeliveryTag, false);                
            } catch (Exception E){                
                // Или любой другой поток лога (база например)
                Console.WriteLine(MSConsts.S_RESP_ERR_STR, E.Message);
            }            
        }
        #region  Disposing
        private bool              ffDisposedValue;        

        /**<summary>деструктор</summary>**/
        ~TSendMailSrv() => Dispose(false);
    
        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool _fDisposing)
        {
            if (!ffDisposedValue){
                if (_fDisposing) {                    
                    if(fpChanel != null)  fpChanel.Dispose();
                    if(fpConnection != null)  fpConnection.Dispose();
                }
                ffDisposedValue = true;
            }
        }        

        #endregion



    }
}