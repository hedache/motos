using System;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class ServiceImage
    {
        public int Id { get; set; }
        [Display(Name = "Image")] public Guid ImageId { get; set; }
        //TODO: Pending to put the correct paths
        [Display(Name = "Image")] public string ImageFullPath => ImageId == Guid.Empty ? $"https://motos.blob.core.windows.net/images/user.png" : $"https://motos.blob.core.windows.net/categories/{ImageId}";
    }
}