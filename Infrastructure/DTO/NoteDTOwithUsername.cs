using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class NoteDTOwithUsername : NoteDTO
    {
        public NoteDTOwithUsername(Note n) : base(n)
        {
            Username = n.Owner.User.Username;
        }
        public string Username { get; set; }
    }
}
