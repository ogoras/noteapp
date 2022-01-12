using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain
{
    public class Session : IUpdateable<Session>
    {
        [Key]
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime LastActivity { get; set; }

        public void updateValues(Session s)
        {
            User = s.User ?? User;
            LastActivity = s.LastActivity;
        }
    }
}
