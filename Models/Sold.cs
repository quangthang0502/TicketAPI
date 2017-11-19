using System;
using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models
{
    public class Sold
    {
        [Key]
        public int sold_id { get; set; }
        public int TicketTypeID { get; set; }
        public int userID { get; set; }
        public string code { get; set; }
    }
}
