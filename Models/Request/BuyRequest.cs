using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketAPI.Models.Request
{
    public class BuyRequest
    {
        public string event_name { get; set; }
        public string type { get; set; }
    }
}
