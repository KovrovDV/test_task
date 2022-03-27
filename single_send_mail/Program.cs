using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Loader;
using System.IO;

namespace single_send_mail
{
    class Program
    {
        private static TSendMailSrv fpSrv;        
        static void Main(string[] args)
        {
            // Оповещение о выгрузке             
             AssemblyLoadContext.Default.Unloading += SigTermEventHandler;            
            // Загружаем параметры
            TQueueParams pQParams = JsonSerializer.Deserialize<TQueueParams>(File.ReadAllText(MSConsts.S_CONFIG_FILE_NAME));            
            // Загружаем текст письма
            byte[] arMessage = File.ReadAllBytes(MSConsts.S_MESSAGE_FILE_NAME);
            // Запускаем сервер
            fpSrv = new TSendMailSrv() { arMessage = arMessage};
            if(fpSrv.Init(pQParams)){
                Console.WriteLine(MSConsts.S_SEND_MAIL_SRV_STARTED);
            }
        }
        /**<summary>Обработка выгррузки</summary>
         * <param name="_pContext">Контекст выгрузки</param>
         * **/
        private static void SigTermEventHandler(AssemblyLoadContext _pContext) {
            if(fpSrv != null) fpSrv.Dispose();
        }        

    }


}
