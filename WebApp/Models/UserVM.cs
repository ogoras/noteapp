using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class UserVM : UserPost
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLoggedIn { get; set; }
    }
}
