using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using TicketAPI.Models.Request;
using System.Security.Claims;
using TicketAPI.Models;

namespace TicketAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/UserReview")]
    public class UserReviewController : Controller
    {
        private readonly IUser _user;
        private readonly IUserReview _user_review;

        public UserReviewController(IUser user,IUserReview userReview)
        {
            _user = user;
            _user_review = userReview;
        }

        [Authorize]
        [HttpPost("rate")]
        public async Task<IActionResult> Rate(RateRequest rateRequest)
        {
            try
            {
                
                string currentUsername = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(currentUsername);
                UserReview userReview = new UserReview()
                {
                    userID = rateRequest.userID,
                    userReview1 = user.User_id,
                    comment = rateRequest.comment,
                    rate = rateRequest.rate
                };

                await _user_review.Add(userReview);
                return Ok(userReview);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [HttpGet("userRate/{username}")]
        public async Task<IActionResult> userRate(string username)
        {
            try
            {
                Users user = await _user.FindByUserName(username);
                if(user == null)
                {
                    return BadRequest("Account does not exist");
                }
                var review = await _user_review.FindByUserID(user.User_id);
                double count = 0;
                double total = 0;
                if (!review.Any())
                {
                    return Ok(total);
                }
                foreach(UserReview userRevew in review)
                {
                    total = total + userRevew.rate;
                    count = count + 1;
                }
                double result = total / count;
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }
    }
}