using System;
using System.Text;
using System.Runtime.Loader;
using  System.Threading;


namespace single_send_mail{

    /**<summary>Класс констант</summary> **/
    public static class MSConsts {
        /**<summary>очередь отправки</summary>**/
        public const string S_QUEUE_NAME        = "send_mail_queue";
        /**<summary>имя файла параметров очереди</summary>**/
        public const string S_CONFIG_FILE_NAME  = "queue_config.json";
        /**<summary>имя файла письма</summary>**/
        public const string S_MESSAGE_FILE_NAME = "message.bin";


        /**<summary>строка ошибки</summary>**/
        public const string S_PROC_ERR_STR = "Ошибка обработки {0}";        
        /**<summary>строка ошибки</summary>**/
        public const string S_RESP_ERR_STR  = "Ошибка ответа {0}";
        /**<summary>строка</summary>**/
        public const string S_SEND_MAIL_SRV_STARTED = "Сервис отправки писем запущен";
    }

}