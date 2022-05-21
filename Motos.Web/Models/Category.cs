using System;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50)][Required] public string Name { get; set; }
        [Display(Name = "Image")] public Guid ImageId { get; set; }
        //TODO: Pending to put the correct paths
        [Display(Name = "Image")] 
        public string ImageFullPath => ImageId == Guid.Empty ? "$https://motos.blob.core.windows.net/images/user.png" : $"https://motos.blob.core.windows.net/categories/{ImageId}";
    }
}
