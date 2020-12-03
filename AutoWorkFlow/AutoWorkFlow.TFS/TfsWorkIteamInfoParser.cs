using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.TFS
{
    /// <summary>
    /// Парсер объекта JSON с информацией по рабочему элементу
    /// </summary>
    public class TfsWorkIteamInfoParser
    {
        /// <summary>
        /// Получить идентификатор рабочего элемента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseId(string content)
        {
            string mask = "id";
            int index = content.IndexOf(mask) + mask.Length + 2;
            string name = content.Substring(index, 6);
            string[] arr = name.Split(',');
            return arr[0];
        }
        /// <summary>
        /// Получить тип рабочего элемента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseType(string content)
        {
            string mask = "System.WorkItemType";
            int index = content.IndexOf(mask) + mask.Length + 3;
            string name = content.Substring(index, 7);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Получить состояние рабочего элемента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseState(string content)
        {
            string mask = "System.State";
            int index = content.IndexOf(mask) + mask.Length + 3;
            string name = content.Substring(index, 10);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Получить имя рабочего элемента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseName(string content)
        {
            string mask = "System.Title";
            int index = content.IndexOf(mask) + mask.Length + 3;
            string name = content.Substring(index, 50);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Получить номер тикета OTRS рабочего элемента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseTicket(string content)
        {
            string mask = "InduSoft.Ticket";
            int index = content.IndexOf(mask) + mask.Length + 3;
            string name = content.Substring(index, 17);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Получить наименование заказчика
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseCustomer(string content)
        {
            string mask = "InduSoft.Customer";
            int index = content.IndexOf(mask) + mask.Length + 3;
            string name = content.Substring(index, 20);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Конвертировать в TfsWorkIteamInfo
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TfsWorkIteamInfo ConvertToTfsWorkIteamInfo(string content)
        {
            return new TfsWorkIteamInfo()
            {
                Id = parseId(content),
                Type = parseType(content),
                Name = parseName(content),
                State = parseState(content),
                Ticket = parseTicket(content),
                Customer = parseCustomer(content)
            };

        }

    }
}
