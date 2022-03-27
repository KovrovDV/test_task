using System;
using System.Text;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace single_send_mail
{
    /**<summary>параметры очереди</summary>**/
    class TQueueParams {
        /**<summary>хост сервера</summary>**/
       public string sHostName {get; set;} = "localhost";
       /**<summary>порт на сервере</summary>**/
       public int    iPort {get; set; } = 42000; 
       /**<summary>виртуальный хост</summary>**/
       public string sVirtualHast{get; set; } = "";
       /**<summary>мя пользователя</summary>**/
       public string sUserName{get; set; } = "";
       /**<summary>пароль</summary>**/
       public string sPassword{get; set; } = ""; 
    }
}