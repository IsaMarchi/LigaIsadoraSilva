
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaIsadoraSilva.Data.Entities
{
    public class Staff
    {
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
        public string? StaffDutyId { get; set; }
        public StaffDuty? StaffDutiy { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
