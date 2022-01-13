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
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public string TextShort
        {
            get
            {
                if (Text.Length > 90) { return Text.Substring(0, 90) + "..."; }
                else { return Text; }
            }
        }
    }
}
