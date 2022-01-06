using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class RegisterVM : UserPost, IValidatableObject
    {
        public string ConfirmPassword;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Password == ConfirmPassword))
                yield return new ValidationResult("Passwords must match!", new string[] { "Password", "ConfirmPassword" });
            if (!Regex.IsMatch(Password, @"(?=^[^\s]{8,}$)"))
                yield return new ValidationResult("Password must be at least 8 characters long and must not contain whitespace characters, e.g. space, tab", new string[] {"Password"});
            if (!Regex.IsMatch(Password, @"(?=.*\d)"))
                yield return new ValidationResult("Password must contain at least one digit", new string[] { "Password" });
            if (!Regex.IsMatch(Password, @"(?=.*[A-Z])"))
                yield return new ValidationResult("Password must contain at least one capital letter", new string[] { "Password" });
            if (!Regex.IsMatch(Password, @"(?=.*[a-z])"))
                yield return new ValidationResult("Password must contain at least one miniscule letter", new string[] { "Password" });
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+$"))
                yield return new ValidationResult("Invalid email", new string[] { "Email" });
            yield break;
        }

        internal object getUserData()
        {
            return new
            {
                Username,
                Email,
                Password
            };
        }
    }
}
