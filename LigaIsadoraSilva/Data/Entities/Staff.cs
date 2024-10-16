
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaIsadoraSilva.Data.Entities
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public FootballTeam? Club { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [ForeignKey("StaffDuty")]
        public int? StaffDutyId { get; set; }
        public StaffDuty? StaffDuty { get; set; }

        [Display(Name = "Profile Picture")]
        public string? Photo { get; set; }

        [NotMapped] // Esta propriedade não será mapeada no banco de dados
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "~/images/noImage.png";
                }

                return $"https://localhost:44301/{Photo.Substring(1)}";
            }
        }

    }
}
