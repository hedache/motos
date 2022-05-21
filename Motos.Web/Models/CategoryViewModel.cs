using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class CategoryViewModel : Category { [Display(Name = "Image")] public IFormFile ImageFile { get; set; } }
}
