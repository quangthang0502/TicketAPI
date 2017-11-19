namespace TicketAPI.Models.Request
{
    public class EventRequest
    {
        public string event_name { get; set; }
        public System.DateTime open_day { get; set; }
        public int quantity_ticket { get; set; }
        public string description { get; set; }
    }
}
