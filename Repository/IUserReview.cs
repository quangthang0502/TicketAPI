using System.Collections.Generic;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public interface IUserReview : IBaseRepository<UserReview>
    {
        Task<List<UserReview>> FindByUserID(int userID);
        Task<List<UserReview>> FindByUserReviewID(int userReviewID);
    }
}
