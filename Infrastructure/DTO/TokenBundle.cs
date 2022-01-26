using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class TokenBundle
    {
        public string SessionId { get; set; }
        public string JsonWebToken { get; set; }
    }
}
