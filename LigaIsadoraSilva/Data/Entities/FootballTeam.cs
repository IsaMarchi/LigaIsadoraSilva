using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaIsadoraSilva.Data.Entities
{
    public class FootballTeam
    {
        internal object Games;

        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Coach { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Stadium { get; set; }

        public DateTime Foundation { get; set; }

        public int Points { get; set; } = 0;

        [Display(Name = "Matches Played")]
        public int MatchesPlayed { get; set; } = 0;

        [Display(Name = "Logo")]
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

        // Coleção de jogadores que pertencem a este time
        public ICollection<Player>? Players { get; set; } = new List<Player>(); // Inicialização para evitar NullReferenceException

        // Coleções de partidas jogadas em casa e fora
        public ICollection<FootballMatch>? HomeGames { get; set; } = new List<FootballMatch>();
        public ICollection<FootballMatch>? VisitGames { get; set; } = new List<FootballMatch>();

    }
}
