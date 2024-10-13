using LigaIsadoraSilva.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Models
{
    public class EditFootballMatchViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias")]
        [DataType(DataType.DateTime, ErrorMessage = "Insira uma data e hora válidas")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime StartDate { get; set; }

        public bool IsFinalized { get; set; }

        [Required]
        public int HomeTeamId { get; set; }
        public FootballTeam? HomeTeam { get; set; } // Clube que joga em casa

        [Required]
        public int VisitTeamId { get; set; }
        public FootballTeam? VisitTeam { get; set; } // Clube visitante

        [Display(Name = "Home Stadium")]
        public string? HomeStadium => HomeTeam?.Stadium;

        [Display(Name = "Home Goals")]
        public int? HomeGoals { get; set; } // Gols marcados pelo clube da casa

        [Display(Name = "Visit Goals")]
        public int? VisitGoals { get; set; } // Gols marcados pelo clube visitante
    }
}
