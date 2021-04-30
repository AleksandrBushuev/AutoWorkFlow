using AutoWorkFlow.OTRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow
{
    class Program
    {
        static void Main(string[] args)
        {
            var credential = InputUserCredential();

            var baseAddress = "https://support.indusoft.ru/otrs/index.pl";
            IAutorizationService service = new AutorizationService(baseAddress);
            OtrsClient client = new OtrsClient(baseAddress, service);
            client.Login(credential);

            List<OtrsTicketInfo> tickets = client.GetTisketsInfoAsync().Result;


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
