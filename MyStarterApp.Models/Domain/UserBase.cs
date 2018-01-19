using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.Domain
{
    public class UserBase
    {
        public int UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Remember { get; set; }
        public int RoleId { get; set; }
        public bool Confirmed { get; set; }
        public bool Suspended { get; set; }
    }
}
