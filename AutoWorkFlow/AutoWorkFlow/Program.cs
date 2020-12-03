using AutoWorkFlow.OTRS;
using AutoWorkFlow.TFS;
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

            Console.WriteLine("Id тикета");
            string id = Console.ReadLine();
            OtrsTicketInfo info = client.GetTicketInfoAsync(baseAddress, id).Result;

            Console.WriteLine(info.ToString());


            Console.WriteLine("Личный маркер доступа");
            string token = Console.ReadLine();
            TfsClient tfsClient = new TfsClient();

            TfsWorkIteamInfo workIteamInfo = tfsClient.GetWorkIteamInfoAsync("https://tfs.indusoft.ru/tfs/InduSoft/I-DS-RO/", token, info.WorkItem).Result;

            Console.WriteLine(workIteamInfo.ToString());

            Console.ReadKey();
        }



        static UserCredential InputUserCredential()
        {
            Console.WriteLine("Введите логин пользователя OTRS");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль пользователя OTRS");
            string password = Console.ReadLine();

            return new UserCredential(login, password);
        }
    }
}
