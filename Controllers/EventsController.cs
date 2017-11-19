using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using TicketAPI.Models;
using System.Security.Claims;
using TicketAPI.Models.Request;

namespace TicketAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEvent _event;
        private readonly IUser _user;

        public EventsController(IEvent events, IUser user)
        {
            _event = events;
            _user = user;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            try
            {
                var events = await _event.getAll();
                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> create(EventRequest events)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(username);

                Events newEvent = new Events()
                {
                    event_name = events.event_name,
                    description = events.description,
                    create_at = DateTime.Now,
                    open_day = Convert.ToDateTime(events.open_day),
                    quantity_ticket = events.quantity_ticket,
                    user_id = user.User_id
                };
                await _event.Add(newEvent);
                return Ok(newEvent);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpGet("get/{event_name}")]
        public async Task<IActionResult> getEvent(string event_name)
        {
            try
            {
                Events events = await _event.FindByName(event_name);
                if(events == null)
                {
                    return NotFound("Event does not exist");
                }
                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<IActionResult> editEvent(EventRequest eventRequest)
        {
            try
            {
                Events Uevent = await _event.FindByName(eventRequest.event_name);
                if (Uevent == null)
                {
                    return NotFound("NotFound");
                }

                string username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(username);

                if(user.User_id != Uevent.user_id)
                {
                    return NotFound("Su kien khong phai cua ban");
                }

                Uevent.event_name = eventRequest.event_name != null ? eventRequest.event_name : Uevent.event_name;
                Uevent.description = eventRequest.description != null ? eventRequest.description : Uevent.description;
                Uevent.quantity_ticket = eventRequest.quantity_ticket != null ? eventRequest.quantity_ticket : Uevent.quantity_ticket;
                DateTime Topen_day = Convert.ToDateTime(eventRequest.open_day);
                Uevent.open_day = Topen_day != null ? Topen_day : Uevent.open_day;

                await _event.Update(Uevent.event_id, Uevent);

                return Ok(Uevent);
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }
    }
}