using System;
using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models
{
    public class Events
    {
        [Key]
        public int event_id { get; set; }
        public int user_id { get; set; }
        public string event_name { get; set; }
        public System.DateTime open_day { get; set; }
        public int quantity_ticket { get; set; }
        public string description { get; set; }
        public DateTime create_at { get; set; }
    }
}
