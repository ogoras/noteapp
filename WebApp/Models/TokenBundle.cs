using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class TokenBundle
    {
        public string SessionId { get; set; }
        public string JsonWebToken { get; set; }
    }
}
