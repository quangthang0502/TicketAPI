using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class UserReviewRepository : Repository<UserReview>, IUserReview
    {
        public UserReviewRepository(AppContextDB context) : base(context)
        {
        }

        async Task<List<UserReview>> IUserReview.FindByUserID(int userID)
        {
            return await _context.user_review.Where(u => u.userID == userID).ToListAsync();
        }

        async Task<List<UserReview>> IUserReview.FindByUserReviewID(int userReviewID)
        {
            return await _context.user_review.Where(u => u.userReview1 == userReviewID).ToListAsync();
        }
    }
}
