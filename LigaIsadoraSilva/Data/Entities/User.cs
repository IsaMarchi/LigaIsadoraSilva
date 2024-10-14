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

        [Display(Name = "Profile Picture")]
        public string? ImageUrl { get; set; }

        public string? ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return "~/images/noImage.png";
                }

                return $"https://localhost:44301/{ImageUrl.Substring(1)}";
            }
        }

    }
}
