using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Request
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

    }
}
