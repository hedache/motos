using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class AddServiceImageViewModel
    {
        public int ServiceId { get; set; }

        [Display(Name = "Image")]
        [Required]
        public IFormFile ImageFile { get; set; }

    }
}
