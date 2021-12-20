using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Profile
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Note? Bio { get; set; }
        public int? BioId { get; set; }
        public Photo? ProfilePicture { get; set; }
        public int? ProfilePictureId { get; set; }
        public List<Note>? Notes { get; set; }
        public List<Note>? NoteShares { get; set; }
        public List<Photo>? Photos { get; set; }
        public List<Photo>? PhotoShares { get; set; }
    }
}
