using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class NoteVM
    {
        [Display(Name = "Encrypt")]
        public bool Encrypted { get; set; }
        public string Key { get; set; }

        [Display(Name="Share publically")]
        public bool? SharedPublically { get; set; }
        public string Text { get; set; }
    }
}
