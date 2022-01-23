using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ChangePasswordVM : IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(NewPassword == ConfirmPassword))
                yield return new ValidationResult("Passwords must match!", new string[] { "NewPassword", "ConfirmPassword" });
            if (!Regex.IsMatch(NewPassword, @"(?=^[^\s]{8,}$)"))
                yield return new ValidationResult("Password must be at least 8 characters long and must not contain whitespace characters, e.g. space, tab", new string[] { "NewPassword" });
            if (!Regex.IsMatch(NewPassword, @"(?=.*\d)"))
                yield return new ValidationResult("Password must contain at least one digit", new string[] { "NewPassword" });
            if (!Regex.IsMatch(NewPassword, @"(?=.*[A-Z])"))
                yield return new ValidationResult("Password must contain at least one capital letter", new string[] { "NewPassword" });
            if (!Regex.IsMatch(NewPassword, @"(?=.*[a-z])"))
                yield return new ValidationResult("Password must contain at least one miniscule letter", new string[] { "NewPassword" });
            yield break;
        }
    }
}
