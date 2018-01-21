using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Services.Authentication.Security.PlainText
{
    public class SimpleClaim
    {
        public string Type { get; set; }

        public string Value { get; set; }


        public SimpleClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }

    }
}
