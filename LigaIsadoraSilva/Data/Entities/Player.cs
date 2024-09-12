using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Data.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public DateTime Birth { get; set; }

        [Required]
        public string Nationality { get; set; }

        [StringLength(400)]
        public string Photo { get; set; }

        [Required]
        public FootballTeam Team { get; set; }

    }
}
