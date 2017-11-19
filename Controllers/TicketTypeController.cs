using Microsoft.AspNetCore.Mvc;
using TicketAPI.Repository;
using TicketAPI.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using TicketAPI.Models.Request;
using System.Security.Claims;

namespace TicketAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/TicketType")]
    public class TicketTypeController : Controller
    {
        private readonly ITicketType _ticket_type;
        private readonly IEvent _event;
        private readonly IUser _user;

        public TicketTypeController(ITicketType ticketType,IEvent events, IUser user)
        {
            _ticket_type = ticketType;
            _event = events;
            _user = user;
        }

        [HttpGet("get/{event_name}")]
        public async Task<IActionResult> get(string event_name)
        {
            try
            {
                Events events = await _event.FindByName(event_name);
                if(events == null)
                {
                    return NotFound("Failed");
                }
                var listTicket = await _ticket_type.FindByEvent(events.event_id);
                return Ok(listTicket);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> create(TypeRequest typeRequest)
        {
            try
            {
                Events Uevent = await _event.FindByName(typeRequest.event_name);
                if (Uevent == null)
                {
                    return NotFound("NotFound");
                }

                string username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(username);

                if (user.User_id != Uevent.user_id)
                {
                    return BadRequest("Su kien khong phai cua ban");
                }

                if (await checkQuantilyAsync(typeRequest.event_name, typeRequest.quantity) == false)
                {
                    return BadRequest("Vuot qua so ve");
                }

                TicketType newType = new TicketType() {
                    event_id = Uevent.event_id,
                    price = typeRequest.price,
                    quantity = typeRequest.quantity,
                    quantity_in_stock = typeRequest.quantity,
                    type = typeRequest.type
                };

                await _ticket_type.Add(newType);
                return Ok(newType);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        private async Task<bool> checkQuantilyAsync(string event_name, int quantily)
        {
            Events events = await _event.FindByName(event_name);
            var listType = await _ticket_type.FindByEvent(events.event_id);
            int total = quantily;
            foreach(TicketType a in listType)
            {
                total = total + a.quantity;
            }
            if(total > events.quantity_ticket)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }
    }
}