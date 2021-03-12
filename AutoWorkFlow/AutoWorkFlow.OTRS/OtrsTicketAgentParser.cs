using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.OTRS
{
    class OtrsTicketAgentParser
    {
        public OtrsTicketAgentParser()
        {

        }

        /// <summary>
        /// Получить количество тикетов пользователя
        /// </summary>
        /// <param name="page">Страница /otrs/index.pl?Action=AgentTicketResponsibleView; </param>
        /// <returns></returns>
        public int GetTicketCount(string page)
        {
            string mask = "Ответственные заявки: Всего:";
            int index = page.IndexOf(mask) + mask.Length;
            string name = page.Substring(index, 3);
            string[] arr = name.Split('\0');

            int count = Convert.ToInt32(arr[0]);

            return count;
        }

        /// <summary>
        /// Получить идентификаторы тикетов 
        /// </summary>
        /// <param name="page">Страница /otrs/index.pl?Action=AgentTicketResponsibleView; </param>
        /// <returns></returns>
        public List<int> GetTicketsId(string page)
        {
            List<int> ids = new List<int>();
            string startMask = "<tbody>";
            string endMask = "</tbody>";
            int indexStart = page.IndexOf(startMask) + startMask.Length;
            int indexEnd = page.IndexOf(endMask);
            string table = page.Substring(indexStart, indexEnd - indexStart);
            string[] text = table.Split(new string[] { "TicketID_" }, StringSplitOptions.None);


            for (int i = 1; i < text.Length; i++)
            {
                string temp = text[i].Substring(0, 6);
                int id = Convert.ToInt32(temp.Split('"')[0]);
                ids.Add(id);
            }

            return ids;
        }




    }
}
