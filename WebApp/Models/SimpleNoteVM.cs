using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class SimpleNoteVM
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
