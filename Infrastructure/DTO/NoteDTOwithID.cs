using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class NoteDTOwithID : NoteDTOwithUsername
    {
        public NoteDTOwithID(Note n) : base(n)
        {
            Id = n.Id;
        }
        public int Id { get; set; }
    }
}
