using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Motos.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Dni { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
        //[Display(Name = "Image")] public Guid ImageId { get; set; }
        ////TODO: Pending to put the correct paths
        //[Display(Name = "Image")] 
        //public string ImageFullPath => ImageId == Guid.Empty ? "$https://localhost:44390/images/user.png" : $"https://tiendaonline.Web.blob.core.windows.net/categories/{ImageId}";
        public ICollection<Registry> Registries { get; set; }
        [DisplayName("Registries Number")]
        public int RegistriesNumber => Registries == null ? 0 : Registries.Count;

        [JsonIgnore] //lo ignora en la respuesta json
        [NotMapped] //no se crea en la base de datos
        public int IdRol { get; set; }

        //public ICollection<Role> Role { get; set; }
    }
}
