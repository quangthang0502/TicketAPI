using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class TicketTypeRepository : Repository<TicketType>, ITicketType
    {
        public TicketTypeRepository(AppContextDB context) : base(context)
        {
        }

        async Task<List<TicketType>> ITicketType.FindByEvent(int event_id)
        {
            return await _context.ticket_type.Where(t => t.event_id == event_id).ToListAsync();
        }

        async Task<TicketType> ITicketType.FindByType(int event_id, string type)
        {

            return await _context.ticket_type.Where(t => t.event_id == event_id && t.type == type).FirstOrDefaultAsync();
        }
    }
}
