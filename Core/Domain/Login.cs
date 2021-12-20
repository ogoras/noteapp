using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Login : IUpdateable<Login>
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public String IP { get; set; }
        public String ClientName { get; set; }

        public void updateValues(Login l)
        {
            User = l.User == null ? User : l.User;
            Date = l.Date;
            Success = l.Success;
            IP = l.IP;
            ClientName = l.ClientName;
        }
    }
}
