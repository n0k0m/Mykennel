using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    public class Litter
    {
        [Display(Name = "Id")]
        public int LitterId { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Kennel")]
        public int KennelId { get; set; }

        [Required]
        [Display(Name = "Father")]
        public int? FatherId { get; set; }

        [Required]
        [Display(Name = "Mother")]
        public int? MotherId { get; set; }

        // Navigációs tulajdonságok:
        [ForeignKey("KennelId")]
        public virtual Kennel Kennel { get; set; }
        public virtual ICollection<Puppy> Puppies { get; set; }

        [ForeignKey("FatherId")]
        public virtual Dog Father { get; set; }
        [ForeignKey("MotherId")]
        public virtual Dog Mother { get; set; }

        // Tárolni lehet az objektumban a fajtát és az országot a szűréshez.
        [NotMapped]
        public Breed Breed { get; set; }
        [NotMapped]
        public Country Country { get; set; }
        [NotMapped]
        public IQueryable<Puppy> Males { get; set; }
        [NotMapped]
        public IQueryable<Puppy> Females { get; set; }
    }
}
