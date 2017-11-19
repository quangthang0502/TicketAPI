using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public interface ISold : IBaseRepository<Sold>
    {
        Task<List<Sold>> FindByTicketType(int type_id);
        Task<object> purchasedTickets(int user_id);
    }
}
