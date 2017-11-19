namespace TicketAPI.Models.Request
{
    public class TypeRequest
    {
        public string event_name { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
