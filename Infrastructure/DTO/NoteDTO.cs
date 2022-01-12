using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class NoteDTO
    {
        public NoteDTO() { }
        public NoteDTO(Note n)
        {
            Uid = n.Owner.UserId;
            Text = n.Text;
        }

        public int? Uid { get; set; }
        public string Text { get; set; }
        public bool Encrypted { get; set; }
        public bool? SharedPublically { get; set; }
    }
}
