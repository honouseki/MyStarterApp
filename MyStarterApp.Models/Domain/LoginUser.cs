using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.Domain
{
    public class LoginUser
    {
        public int Id { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Username is too long")]
        public string Username { get; set; }
        [Required, EmailAddress(ErrorMessage = "Invalid email-address")]
        public string Email { get; set; }
        public string Salt { get; set; }
        public string HashPassword { get; set; }
        public int MyProperty { get; set; }
        [Required, MinLength(6, ErrorMessage = "Password requires a minimum of 6 characters")]
        //[RegularExpression(@"^[a-zA-Z][0-9]$", ErrorMessage = "Does not contain a letter AND a number")]
        public string Password { get; set; }
    }
}
