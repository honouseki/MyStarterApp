using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models
{
    // Return type for login, indicating success/failure as well as the reason behind it
    public enum LoginType
    {
        // Login successful
        Success = 1,
        // Incorrect credentials were entered
        Incorrect = 0,
        // Indicates a login attempt with a suspended account
        Suspended = -1,
        // Indicates a login attempt with a deactivated account; Currently not in use
        //deactivated = -2
        Other = -10
    }
}
