using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Data.Entities
{
    public class FootballTeam
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Coach { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Stadium { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Logo")]
        public string LogoUrl { get; set; } 

        public DateTime Foundation { get; set; }

        public int Points { get; set; }

        [Display(Name = "Matches Played")]
        public int MatchesPlayed { get; set; }

        public ICollection<Player> Players { get; set; }
        public ICollection<FootballMatch> HomeGames { get; set; }
        public ICollection<FootballMatch> AwayGames { get; set; }      

    }
}
