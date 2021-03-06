using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class UserPost : LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public new string Password { get; set; }
    }
}
