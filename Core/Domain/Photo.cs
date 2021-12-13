using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    class Photo
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public string Url { get; set; }
        public Note? Note { get; set; }
    }
}
