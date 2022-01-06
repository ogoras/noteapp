using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class UserDTO : LoginDTO
    {
        public UserDTO() { }
        public UserDTO(User u)
        {
            Username = u.Username;
            Email = u.Email;
            Password = u.Password;
        }
        public String? Email { get; set; }
    }
}
