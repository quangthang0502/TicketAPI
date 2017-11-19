using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class EventRepository : Repository<Events>, IEvent
    {
        public EventRepository(AppContextDB context) : base(context)
        {
        }

        async Task<Events> IEvent.FindByName(string event_name)
        {
            return await _context.events.Where(x => x.event_name.Equals(event_name)).FirstOrDefaultAsync();
        }

        async Task<List<Events>> IEvent.FindByUser(int user_id)
        {
            return await _context.events.Where(x => x.user_id == user_id).ToListAsync();
        }

        async Task<object> IEvent.FindUser(int event_id)
        {
            var result = from events in _context.events
                         join user in _context.user on events.user_id equals user.User_id
                         where events.event_id == event_id
                         select new
                         {
                             user.username,
                             user.fullname,
                             user.email,
                             user.birthday,
                             user.phone
                         };
            return result;
        }

        async Task<List<Events>> IEvent.search(string title, int limit, int page)
        {
            return await _context.events.Where(e => e.event_name.Contains(title))
                .Take(limit)
                .Skip((page - 1) * limit)
                .ToListAsync();
        }
    }
}
