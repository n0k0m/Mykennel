using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    public enum Aim
    {
        [Display(Name = "Hobby")] Hobby = 1,
        [Display(Name = "Show")] Show = 2,
        [Display(Name = "Other")] Other = 3
    }
    public class Puppy
    {
        [Display(Name = "Id")]
        public int PuppyId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; } // Nem azonos a felnőttkori névvel

        [Required]
        [Range(1, 2)]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; }

        [Required]
        [Display(Name = "Bookable")]
        public bool Bookable { get; set; }

        [Required]
        [Range(1, 3)]
        [Display(Name = "Aim")]
        public Aim Aim { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } // További leírás megadásához

        [Display(Name = "Link")]
        public int? DogId { get; set; }
        [Display(Name = "Litter")]
        public int LitterId { get; set; }

        // Navigációs tulajdonságok:
        [ForeignKey("DogId")]
        public virtual Dog Dog { get; set; }
        [ForeignKey("LitterId")]
        public virtual Litter Litter { get; set; }
    }
}
