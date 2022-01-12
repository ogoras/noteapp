using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class NoteWithIDVM : NoteVM
    {
        public int Id { get; set; }
        public string OwnerUsername { get; set; }
    }
}
