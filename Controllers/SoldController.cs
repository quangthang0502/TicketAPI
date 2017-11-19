using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TicketAPI.Models;

namespace TicketAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Sold")]
    public class SoldController : Controller
    {
        private readonly ISold _sold;
        private readonly IEvent _event;
        private readonly ITicketType _ticket_type;
        private readonly IUser _user;

        public SoldController(ISold sold,IEvent events,ITicketType ticket,IUser user)
        {
            _sold = sold;
            _event = events;
            _ticket_type = ticket;
            _user = user;
        }

        [Authorize]
        [HttpGet("buyTicket/{event_name}/{type}")]
        public async Task<IActionResult> buyTicket(string event_name,string type)
        {
            try
            {
                string currentUsername = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(currentUsername);

                Events events = await _event.FindByName(event_name);
                if(events == null)
                {
                    return NotFound("Event does not exist");
                }
                
                if(events.user_id == user.User_id)
                {
                    return NotFound("Su kien nay la cua ban");
                }

                TicketType ticket = await _ticket_type.FindByType(events.event_id, type);

                if (ticket == null)
                {
                    return NotFound("Type does not exist");
                }

                Sold newSold = new Sold()
                {
                    TicketTypeID = ticket.ticket_type_id,
                    code = genCode(128),
                    userID = user.User_id
                };

                await _sold.Add(newSold);
                ticket.quantity_in_stock = ticket.quantity_in_stock - 1;
                await _ticket_type.Update(ticket.ticket_type_id, ticket);
                return Ok(newSold);
            }
            catch (Exception e)
            {
                return BadRequest("Failed"); 
            }
        }

        [Authorize]
        [HttpGet("purchasedTickets")]
        public async Task<IActionResult> purchasedTickets()
        {
            try
            {
                string currentUsername = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(currentUsername);
                var x = await _sold.purchasedTickets(user.User_id);
                return Ok(x);
            }
            catch(Exception e)
            {
                return BadRequest("Failed");
            }
        }

        private string genCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}