using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    public class Breed
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] // Kikapcsolja az automatikus növekményt az elsődleges kulcson
        [Display(Name = "FCI number")]
        public int BreedId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Original name")]
        public string OriginalName { get; set; }

        // Navigációs tulajdonságok:
        public virtual ICollection<Dog> Dogs { get; set; }
    }
}
