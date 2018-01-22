using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.Interfaces
{
    public interface IUserAuthData
    {
        int UserId { get; set; }
        string Username { get; set; }
        bool Remember { get; set; }
    }
}
