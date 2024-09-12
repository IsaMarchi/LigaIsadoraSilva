using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{Surname} {Name}";

    }
}
