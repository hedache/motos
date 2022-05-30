using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://motos.blob.core.windows.net/images/user.png"
            : $"https://motos.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        //[Required]
        //[Display(Name = "Positions")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a Positions.")]
        //public int PositionId { get; set; }

        //public IEnumerable<SelectListItem> Positions { get; set; }

        //[Required]
        //[Display(Name = "Person")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a person.")]
        //public int PersonId { get; set; }

        //public IEnumerable<SelectListItem> Persons { get; set; }

        //[Required]
        //[Display(Name = "Registry")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a registry.")]
        //public int RegistryId { get; set; }

        //public IEnumerable<SelectListItem> Registries { get; set; }

    }
}
