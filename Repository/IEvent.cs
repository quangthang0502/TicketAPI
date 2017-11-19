using System.Collections.Generic;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public interface IEvent : IBaseRepository<Events>
    {
        Task<List<Events>> FindByUser(int user_id);
        Task<List<Events>> search(string title, int limit, int page);
        Task<object> FindUser(int event_id);
        Task<Events> FindByName(string event_name);
        //Task<List<Events>> getAllEvent();
    }
}
