using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage ="El campo {0} debe contener al menos un caracter")]
        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }

        [DisplayName("Cantidad de usuarios")]
        public int PersonsNumber => Persons == null ? 0 : Persons.Count;
    }
}
