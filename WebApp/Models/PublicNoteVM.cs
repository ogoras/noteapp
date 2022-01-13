using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PublicNoteVM
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
    }
}
