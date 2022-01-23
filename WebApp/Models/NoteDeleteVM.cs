using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class NoteDeleteVM : SimpleNoteVM
    {
        [Display(Name = "Encrypt")]
        public bool Encrypted { get; set; }

        [Display(Name = "Share publically")]
        public bool? SharedPublically { get; set; }
    }
}
