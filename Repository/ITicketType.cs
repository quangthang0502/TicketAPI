using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public interface ITicketType : IBaseRepository<TicketType>
    {
        Task<List<TicketType>> FindByEvent(int event_id);
        Task<TicketType> FindByType(int event_id, string type);
    }
}
