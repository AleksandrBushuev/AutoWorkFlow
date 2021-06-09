using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Парсер страницы Html с информацией по тикету
    /// </summary>
    class OtrsTicketInfoParser
    {

        /// <summary>
        /// Конвертировать страницу HTML в TicketInfo
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static void parse(string content, out OtrsTicketInfo ticketInfo)
        {
            string id = parseId(content);
            string number = parseNumber(content);
            string name = parseName(content);
            string state = parseState(content);
            List<string> workItems = parseWorkItem(content);
            ticketInfo = new OtrsTicketInfo(id, number, name, state, workItems);
        }
        
        /// <summary>
        /// Получить идентификатор тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string parseId(string content)
        {
            Regex regex = new Regex(@"(?<=TicketID=)\d+");
            MatchCollection match = regex.Matches(content);
            if (match.Count == 0)
            {
                return string.Empty;
            }

            return match[0].Value;
        }
        /// <summary>
        /// Получить номер тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string parseNumber(string content)
        {           
            Regex regex = new Regex(@"(?<=Подробно Ticket#)\d+");
            MatchCollection match = regex.Matches(content);
            if (match.Count == 0)
            {
                return string.Empty;
            }

            return match[0].Value;            
        }
        /// <summary>
        /// Получить имя тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string parseName(string content)
        {
            Regex regex = new Regex(@"[\w\s|\.\,\!\?\-\:]+(?=</h1)");
            MatchCollection match = regex.Matches(content);
            if (match.Count == 0)
            {
                return string.Empty;
            }
            return match[0].Value;
        }

        /// <summary>
        /// Получить статус тикета
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string parseState(string content)
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
        private static List<string> parseWorkItem(string content)
        {
            List<string> result = new List<string>();

            Regex regex = new Regex(@"(WorkItem[\w\W]+)");
            MatchCollection match = regex.Matches(content);
            if (match.Count == 0)
                return result;

            int indexEnd = (match[0].Value).IndexOf("</span>");

            if (indexEnd == -1)
                return result;

            string substring = (match[0].Value).Substring(0, indexEnd);
            Regex regexNumber = new Regex(@"(\d+)");
            MatchCollection matchNumbers = regexNumber.Matches(substring);

            if (matchNumbers.Count == 0)
                return result;

            foreach(Match item in matchNumbers)
            {
                result.Add(item.Value);
            }

            return result.Distinct().ToList();
        }         
    }
}
