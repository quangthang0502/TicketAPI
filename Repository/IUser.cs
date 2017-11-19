using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public interface IUser : IBaseRepository<Users>
    {
        Task<Users> FindByUserName(string username);
    }
}
