using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] // Kikapcsolja az automatikus növekményt az elsődleges kulcson
        [Display(Name = "ISO number")]
        public int CountryId { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string CountryName { get; set; }

        // Navigációs tulajdonságok:
        public virtual ICollection<Kennel> Kennels { get; set; }
    }
}
