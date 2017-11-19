using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.App_data
{
    public static class DbInitializer
    {
        public static void Initialize(AppContextDB context)
        {
            context.Database.EnsureCreated();
            if (context.user.Any())
            {
                return;
            }
            var user = new Users[]
            {
                new Users{username="quangthang",fullname="thangdeptrai",password="123",role="user"},
                new Users{username="quangthang1",fullname="thangdeptrai1",password="1234",role="admin"},
                new Users{username="quangthang123",fullname="thangdeptrai1",password="1234",role="user"},
                new Users{username="quangthang1234",fullname="thangdeptrai1",password="12345",role="user"}
            };
            foreach (Users s in user)
            {
                context.user.Add(s);
            }

  
            context.SaveChanges();
        }
    }
}
