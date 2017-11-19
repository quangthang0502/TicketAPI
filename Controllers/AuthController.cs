using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Models;
using TicketAPI.Repository;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TicketAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUser _user;
        private string secrectKey = "quangthang_dmteam_tickets";

        public AuthController(IUser user)
        {
            _user = user;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromHeader] String Authorization)
        {
            try
            {
                string auth = Encoding.UTF8.GetString(Convert.FromBase64String(Authorization.Substring(5)));
                string username = auth.Substring(0, auth.IndexOf(":"));
                string password = auth.Substring((auth.IndexOf(":")+1));

                var user = await _user.FindByUserName(username);
                if(user == null || !user.password.Equals(password))
                {
                    return NotFound("Sai tai khoan hoac mat khau");
                }
                return Ok(new {token = genToken(user)});
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Failed");
            }
        }


        [HttpPost("signup")]
        public async Task<IActionResult> Signup(Users userRequest)
        {
            try
            {
                string username = userRequest.username;
                string password = userRequest.password;
                
                var user = await _user.FindByUserName(username);

                if (user != null)
                {
                    return NotFound("Username does exist");
                }

                var newuser = new Users
                {
                    username = username,
                    password = password,
                    birthday = userRequest.birthday,
                    fullname = userRequest.fullname,
                    phone = userRequest.phone,
                    email = userRequest.email,
                    paypalAcount = userRequest.paypalAcount,
                    role = "user"
                };

                await _user.Add(newuser);
                return Ok(new { token = genToken(newuser) });
            }
            catch (Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> getProfile()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(username);

                var dict = new Dictionary<string, object>();

                dict.Add("username", user.username);
                dict.Add("fullname", user.fullname);
                dict.Add("phone", user.phone);
                dict.Add("email", user.email);
                dict.Add("birthday", user.birthday);
                dict.Add("paypalAcount", user.paypalAcount);
                return Ok(dict);
            }
            catch(Exception e)
            {
                return BadRequest("Failed");
            }
        }

        [Authorize]
        [HttpPut("profile/edit")]
        public async Task<IActionResult> editProfile(string fullname,string phone,string email,string birthday,string paypalAcount)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = await _user.FindByUserName(currentUserName);
                if (user == null)
                {
                    return NotFound("NotFound");
                }
                user.fullname = fullname != null ? fullname : user.fullname;
                user.phone = phone != null ? phone : user.phone;
                user.email = email != null ? email : user.email;
                DateTime tBirthday = Convert.ToDateTime(birthday);
                user.birthday = tBirthday;
                user.birthday = tBirthday != null ? tBirthday : user.birthday; 
                user.paypalAcount = paypalAcount != null ? paypalAcount : user.paypalAcount;

                await _user.Update(user.User_id, user);
                return Ok("OK");
            }
            catch (Exception e)
            {
                return BadRequest("null");
            }
        }

        private object genToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(this.secrectKey));
            var claim = new Claim[] {
                new Claim(JwtRegisteredClaimNames.NameId,user.username),
                new Claim(JwtRegisteredClaimNames.Jti,user.username),
                new Claim(ClaimTypes.Role,user.role),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };
            var token = new JwtSecurityToken(
                    claims: claim,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }
    }
}