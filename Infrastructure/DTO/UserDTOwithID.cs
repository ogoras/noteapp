using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class UserDTOwithID : UserDTO
    {
        public UserDTOwithID(User u) : base(u)
        {
            Id = u.Uid;
        }
        public int Id { get; set; }
    }
}
