using System;
using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models
{
    public class Users
    {
        [Key]
        public int User_id { get; set; }
        [MaxLength(25),Required]
        public string username { get; set; }
        [MaxLength(20), Required]
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public System.DateTime birthday { get; set; }
        public string paypalAcount { get; set; }
        public string role { get; set; }
    }
}
