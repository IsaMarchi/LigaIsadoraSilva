using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Data.Entities
{
    public class PayersPosition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }
    }
}
