using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class DecryptVM
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public string Key { get; set; }
    }
}
