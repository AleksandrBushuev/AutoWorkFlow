using AutoWorkFlow.OTRS;
using AutoWorkFlow.Serializable;
using AutoWorkFlow.TFS;
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
			var baseAddress = "https://support.indusoft.ru/otrs/index.pl";
            var credential = InputUserCredential();
         
            IAutorizationService service = new AutorizationService(baseAddress);
            OtrsClient client = new OtrsClient(baseAddress, service);
            client.Login(credential);

            List<OtrsTicketInfo> tickets = client.GetTisketsInfoAsync().Result;

            XmlSerializerHelper<List<OtrsTicketInfo>> serializerHelper = new XmlSerializerHelper<List<OtrsTicketInfo>>();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Тикеты.xml");

            serializerHelper.Serialize(path, ticketInfos);

            List<OtrsTicketInfo> tickets = serializerHelper.Deserialize(path);


            Console.ReadKey();
        }


        private static UserCredential InputUserCredential()
        {
            Console.WriteLine("Введите логин пользователя OTRS");
            string login = Console.ReadLine();
            Console.WriteLine("Введите логин пользователя OTRS");
            string password = Console.ReadLine();
            return new UserCredential(login, password);
        }
       
    }
}
