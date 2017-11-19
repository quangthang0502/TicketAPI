namespace TicketAPI.Models.Request
{
    public class RateRequest
    {
        public int userID { get; set; }
        public double rate { get; set; }
        public string comment { get; set; }
    }
}
