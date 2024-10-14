using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaIsadoraSilva.Data.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public FootballTeam? Club { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }


        [Required(ErrorMessage = "The date and time are mandatory")]
        [DataType(DataType.DateTime, ErrorMessage = "Enter a valid date and time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birth { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Display(Name = "Profile Picture")]
        public string? Photo { get; set; }

        [ForeignKey("PlayersPosition")]
        public int? PlayersPositionId { get; set; }
        public PayersPosition? PlayersPosition { get; set; }

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

