using System;
using System.Collections.Generic;

namespace AutoWorkFlow.OTRS
{
    /// <summary>
    /// Информация по тикету OTRS
    /// </summary>    
    [Serializable]
    public class OtrsTicketInfo
    {
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public List<string> WorkItems { get; set; }

        public OtrsTicketInfo() { }
        public OtrsTicketInfo(string id, string number, string name, string state, List<string> workItems)
        {
            this.ID = id;
            this.Number = number;
            this.Name = name;
            this.State = state;
            this.WorkItems = workItems;
        }

        public override string ToString()
        {
            var workItems = WorkItemsToString(this.WorkItems);
            return string.Format(" Id:\t{0}\n Number:\t{1}\n Name:\t{2}\n State:\t{3}\n WorkItem:\t{4}\n", ID, Number, Name, State, workItems);
        }


        private string WorkItemsToString(List<string> workItems)
        {
            string result = string.Empty;
            if (WorkItems.Count == 0)
                return result;
                        
            workItems.ForEach(item =>
            {
                result += $"{item}, ";
            });

            result = result.Remove(result.Length - 2, 1);

            return result;
        }


    }
}
