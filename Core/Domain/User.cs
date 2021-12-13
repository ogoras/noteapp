using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain
{
    class User
    {
        [Key]
        public int Uid { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; } //The user password and hash
        public DateTime DateCreated { get; set; }
        public UserLogin? LastLogin { get; set; }
        public List<UserLogin> UserLogins { get; set; }
        public Profile Profile { get; set; }
    }
}
