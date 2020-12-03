using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWorkFlow.TFS
{   /// <summary>
    /// Класс описывающий рабочий элемент TFS
    /// </summary>
    public class TfsWorkIteamInfo
    {
        public TfsWorkIteamInfo() { }
        public TfsWorkIteamInfo(string id, string type, string state, string name, string ticket, string customer)
        {
            this.Id = id;
            this.Type = type;
            this.State = state;
            this.Name = name;
            this.Ticket = ticket;
            this.Customer = customer;
        }
        public string Id { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public string Ticket { get; set; }
        public string Customer { get; set; }

        public override string ToString()
        {
            return string.Format("Id:\t{0}\nType:\t{1}\nState:\t{2}\nName:\t{3}\nTicket:\t{4}\nCustomer:\t{5}", Id, Type, State, Name, Ticket, Customer);
        }
    }
}
