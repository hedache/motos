using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motos.Web.Models
{
    public class Registry
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Dni { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Placa { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Owner { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Description { get; set; }
        //[Required]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        //public int Total { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Status { get; set; }
        [JsonIgnore] //lo ignora en la respuesta json
        [NotMapped] //no se crea en la base de datos
        public int IdUser { get; set; }

    }
}
