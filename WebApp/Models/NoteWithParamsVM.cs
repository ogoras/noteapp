using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class NoteWithParamsVM : NoteVM
    {
        public string OwnerUsername { get; set; }
        public bool Encrypted { get; set; }
        public bool? SharedPublically { get; set; }
    }
}
