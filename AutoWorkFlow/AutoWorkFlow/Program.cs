using AutoWorkFlow.OTRS;
using AutoWorkFlow.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Тикеты.xml");
            var baseAddress = "https://support.indusoft.ru/otrs/index.pl";
            var credential = InputUserCredential();
         
            IAutorizationService service = new AutorizationService(baseAddress);
            OtrsClient client = new OtrsClient(baseAddress, service);
            client.Login(credential);

            if (!client.IsLogin)
            {
                Console.WriteLine("Не удалось выполнить авторизацию");
            }else
            {
                Console.WriteLine("Выполняется запрос данных. Ожидайте...");
            }

           
            List<OtrsTicketInfo> tickets = client.GetTisketsInfoAsync().Result;
            ExportToXML(path, tickets);







            Console.ReadKey();
        }


        private static UserCredential InputUserCredential()
        {
            Console.WriteLine("Введите логин пользователя OTRS");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль пользователя OTRS");
            string password = Console.ReadLine();
            return new UserCredential(login, password);
        }
       

        private static void ExportToXML(string path, List<OtrsTicketInfo> tickets)
        {
            XmlSerializerHelper<List<OtrsTicketInfo>> serializerHelper = new XmlSerializerHelper<List<OtrsTicketInfo>>();
            serializerHelper.Serialize(path, tickets);
        }


    }
}
