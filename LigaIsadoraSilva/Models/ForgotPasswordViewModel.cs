using System.ComponentModel.DataAnnotations;

namespace LigaIsadoraSilva.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}