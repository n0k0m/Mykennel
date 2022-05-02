using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    // Létrehozok saját User-t, amiben további tulajdonságokat tudok megadni, ami az applikációmhoz kell
    public class ApplicationUser : IdentityUser
    {
        // Nem kerül be az adatbázisba, a Role-ok egyszerűbben kezelhetők, ha eleve tulajdonságként tároljuk
        [NotMapped]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public virtual ICollection<Kennel> Kennels { get; set; }
    }
}
