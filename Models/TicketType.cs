using System;
using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models
{
    public class TicketType
    {
        [Key]
        public int ticket_type_id { get; set; }
        public int event_id { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int quantity_in_stock { get; set; }
    }
}
