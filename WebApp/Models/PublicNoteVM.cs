using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PublicNoteVM : SimpleNoteVM
    {
        public string Username { get; set; }
        
    }
}
