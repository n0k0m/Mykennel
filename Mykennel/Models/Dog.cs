using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    public enum Sex
    {
        [Display(Name = "Male")] Male = 1,
        [Display(Name = "Female")] Female = 2
    }

    public enum Status
    {
        [Display(Name = "Dog in the kennel")] Ours = 1,
        [Display(Name = "Dog from our kennel")] FromUs = 2,
        [Display(Name = "Ancestor of our dog")] Ancestor = 3
    }

    public class Dog
    {
        public int DogId { get; set; }

        [MaxLength(20)]
        [Display(Name = "Registration number")]
        public string RegNumber { get; set; } // Valójában nem szám, hanem betűket is tartalmazhat
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Born")]
        public DateTime Born { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Range(1, 2)]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [MaxLength(150)]
        [Display(Name = "Titles and Genetics")]
        public string TitlesGenetics { get; set; }
        
        [Required]
        [Range(1, 3)]
        [Display(Name = "Status")]
        public Status Status { get; set; }

        [MaxLength(200)]
        [Display(Name = "Image")]
        public string DogImage { get; set; }

        [Display(Name = "Breed")]
        public int BreedId { get; set; }
        [Display(Name = "Kennel")]
        public int KennelId { get; set; }

        // Navigációs tulajdonságok
        [ForeignKey("BreedId")]
        public virtual Breed Breed { get; set; }
        [ForeignKey("KennelId")]
        public virtual Kennel Kennel { get; set; }
        public virtual Puppy Puppy { get; set; }

        // Önmagára irányuló referencia beállítása
        [Display(Name = "Dog's Father")]
        public int? FatherId { get; set; }
        [Display(Name = "Dog's Mother")]
        public int? MotherId { get; set; }

        [ForeignKey("FatherId")]
        public virtual Dog Father { get; set; }
        [ForeignKey("MotherId")]
        public virtual Dog Mother { get; set; }
    }
}
