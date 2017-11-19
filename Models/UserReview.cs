using System;
using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models
{
    public class UserReview
    {
        [Key]
        public int Id { get; set; }
        public int userID { get; set; }
        public int userReview1 { get; set; }
        public double rate { get; set; }
        public string comment { get; set; }
    }
}
