using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Motos.Web.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage ="El campo {0} debe contener al menos un caracter")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        [DisplayName("Cantidad de usuarios")]
        public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}
