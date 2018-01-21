using MyStarterApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.Domain
{
    public class UserBase : IUserAuthData
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string Confirmed { get; set; }
        public string Suspended { get; set; }
        public bool Remember { get; set; }
    }
}
