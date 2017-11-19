using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class SoldRepository : Repository<Sold>, ISold
    {
        public SoldRepository(AppContextDB context) : base(context)
        {
        }

        async Task<List<Sold>> ISold.FindByTicketType(int type_id)
        {
            return await _context.sold.Where(s => s.TicketTypeID == type_id).ToListAsync();
        }

        async Task<object> ISold.purchasedTickets(int user_id)
        {
            var result = from sold in _context.sold
                         join ticket_type in _context.ticket_type on sold.TicketTypeID equals ticket_type.ticket_type_id
                         join events in _context.events on ticket_type.event_id equals events.event_id
                         where sold.userID == user_id
                         select new {
                             events.event_name,
                             ticket_type.type,
                             ticket_type.price,
                             sold.code
                         };
            return result;
        }
    }
}
