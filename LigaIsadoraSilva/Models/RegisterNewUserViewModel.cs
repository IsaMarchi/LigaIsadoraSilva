using LigaIsadoraSilva.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string UserRole { get; set; }

        [Required]
        [Display(Name = "Football Team")]
        public int? FootballTeamId { get; set; }
        public FootballTeam? FootballTeam { get; set; }

        // Lista de Football Teams para o ComboBox
        public List<FootballTeam>? FootballTeams { get; set; }

        // Opções para o tipo de usuário
        public List<string> UserRoles { get; set; } = new List<string> { "Team", "Staff" };
    }
}
