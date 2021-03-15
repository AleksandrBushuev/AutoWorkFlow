using System;

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
        public string WorkItem { get; set; }

        public OtrsTicketInfo() { }
        public OtrsTicketInfo(string id, string number, string name, string state, string workItem)
        {
            this.ID = id;
            this.Number = number;
            this.Name = name;
            this.State = state;
            this.WorkItem = workItem;
        }             

        public override string ToString()
        {
            return string.Format(" Id:\t{0}\n Number:\t{1}\n Name:\t{2}\n State:\t{3}\n WorkItem:\t{4}\n", ID, Number, Name, State, WorkItem);
        }


    }
}
