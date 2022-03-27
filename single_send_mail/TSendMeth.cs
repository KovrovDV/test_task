using System;
using System.Text;
using System.Runtime.Loader;
using  System.Threading;
using  System.Threading.Tasks;
using RabbitMQ.Client;


namespace single_send_mail
{
    /**<summary>Метод для отправки писем</summary>**/
    static class TSendMeth {
      /**<summary>Метод для отправки писем</summary>
        * <param name="_sEmail">Адрес</param>
        * <param name="_arMessage">Сообщение, например в html</param>     
        * ... Заголовок, от кого, aliace от кого, кодировка, ...
        * <remark> Может быть и методом класса - тогда надо передавать внутрь, т.к. скорее всего это SMTP то await уменьшит колво ожидающих потоков</remark>
      **/
      public static async Task<bool> SendMeth(string _sEmail, byte[] _arMessage){
          return await Task.FromResult(true);
      }
    }
}