using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Photo
    {
        public int Id { get; set; }
        public Profile Owner { get; set; }
        public string Url { get; set; }
        public Note? Note { get; set; }
        public List<Profile> ShareRecipients { get; set; }
    }
}
