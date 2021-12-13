using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class UserLogin
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public String IP { get; set; }
        public String CilentName { get; set; }
    }
}
