using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Data.Entities
{
    public class FootballMatch
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        public int HomeTeamId { get; set; }
        public FootballTeam HomeTeam { get; set; } // Clube que joga em casa

        [Required]
        public int HostTeamId { get; set; }
        public FootballTeam HostTeam { get; set; } // Clube visitante

        [Display(Name = "Home Goals")]
        public int HomeGoals { get; set; } // Gols marcados pelo clube da casa

        [Display(Name = "Away Goals")]
        public int AwayGoals { get; set; } // Gols marcados pelo clube visitante


    }
}
