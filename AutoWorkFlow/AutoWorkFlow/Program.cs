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
            var baseAddress = "https://support.indusoft.ru/otrs/index.pl";


            UserCredential credential = InputUserCredential();

            IAutorizationService service = new AutorizationService(baseAddress, credential);
            OtrsClient client = new OtrsClient(service);

            OtrsTicketInfo info = client.GetTicketInfoAsync(baseAddress, "31627").Result;

            Console.WriteLine(info.ToString());

            Console.ReadKey();
        }

        static UserCredential InputUserCredential()
        {
            Console.WriteLine("Введите логин пользователя OTRS");
            string login = Console.ReadLine();
            Console.WriteLine("Введите логин пользователя OTRS");
            string password = Console.ReadLine();

            return new UserCredential(login, password);
        }
    }
}
