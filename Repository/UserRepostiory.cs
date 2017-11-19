using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class UserRepostiory : Repository<Users>, IUser
    {
        public UserRepostiory(AppContextDB context) : base(context)
        {

        }
        async Task<Users> IUser.FindByUserName(string username)
        {
            return await _context.user.Where(u => u.username == username).FirstOrDefaultAsync();
        }
    }
}
