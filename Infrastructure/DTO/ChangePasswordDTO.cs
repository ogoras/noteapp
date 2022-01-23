using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTO
{
    public class ChangePasswordDTO
    {
        public int Uid { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
