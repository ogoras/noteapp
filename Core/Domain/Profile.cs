using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    class Profile
    {
        public User User { get; set; }
        public Note? Bio { get; set; }
        public Photo? ProfilePicture { get; set; }
        public List<Note> Notes { get; set; }
        public List<Note> NotesSharedToUser { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
