using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Парсер страницы Html с информацией по тикету
    /// </summary>
    class OtrsTicketInfoParser
    {
        /// <summary>
        /// Получить идентификатор тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseId(string content)
        {
            string mask = "TicketID=";
            int index = content.IndexOf(mask) + mask.Length;
            string name = content.Substring(index, 6);
            string[] arr = name.Split('"');
            return arr[0];
        }
        /// <summary>
        /// Получить номер тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseNumber(string content)
        {
            string mask = "Подробно Ticket#";
            int index = content.IndexOf(mask) + mask.Length;
            string ticket = content.Substring(index, 16);
            return ticket;
        }
        /// <summary>
        /// Получить имя тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseName(string content)
        {
            string mask = "Подробно Ticket#";
            int index = content.IndexOf(mask) + mask.Length + 25;
            string name = content.Substring(index, 60);
            string[] arr = name.Split('\"');
            return arr[0];
        }

        /// <summary>
        /// Получить статус тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseState(string content)
        {
            string mask = "Состояние";
            int index = content.IndexOf(mask) + 67;
            string state = content.Substring(index, 30);
            string[] arr = state.Split('\"');
            return arr[0];
        }


        /// <summary>
        /// Получить рабочий номер тикета в TFS
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string parseWorkItem(string content)
        {
            string result = "нет";
            string mask = "WorkItem";
            int index = content.IndexOf(mask);

            if (index != -1)
            {
                index += 103;
                string number = content.Substring(index, 8);
                string[] arr = number.Split('\"');
                result = arr[0];
            }
            return result;
        }


        public static string parseChallengeToken(string content)
        {
            string mask = "ChallengeToken";
            int index = content.IndexOf(mask) + mask.Length + 9;
            string token = content.Substring(index, 32);
            return token;
        }

        /// <summary>
        /// Конвертировать страницу HTML в TicketInfo
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static OtrsTicketInfo convertToOtrsTicketInfo(string content)
        {
            string id = parseId(content);
            string number = parseNumber(content);
            string name = parseName(content);
            string state = parseState(content);
            string workItem = parseWorkItem(content);

            return new OtrsTicketInfo(id, number, name, state, workItem);
        }
    }
}
