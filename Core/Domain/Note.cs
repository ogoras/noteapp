using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Note
    {
        public int Id { get; set; }
        public Profile Owner { get; set; }
        public bool Encrypted { get; set; }
        public bool? SharedPublically { get; set; }
        public String Text { get; set; }
        public List<Profile> ShareRecipients { get; set; }
        public List<Photo> AttachedPhotos { get; set; }
    }
}
