using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models
{
    // Beállítja, hogy az URLName csak egyedi lehet
    [Index(nameof(Kennel.URLName), IsUnique = true)]
    public class Kennel
    {
        [Display(Name = "Id")]
        public int KennelId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Kennel Name")]
        public string KennelName { get; set; } // Kennel teljes neve, nem azonos a felhasználónévvel
       
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[a-z0-9]+", ErrorMessage = "Please enter a short name with lowercase letters and numbers only!")]
        [Display(Name = "URL friendly name")]
        public string URLName { get; set; } // Ha Id helyett SEO barát URL címet szeretnénk használni

        [Required]
        [MaxLength(50)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [MaxLength(500)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } // Listás nézethez egy rövidebb bemutatkozó leírás

        [MaxLength(2000)]
        [Display(Name = "Description")]
        public string Description { get; set; } // Kennel oldalán megjelenő hosszabb leírás

        [MaxLength(100)]
        [Display(Name = "Logo")]
        public string Logo { get; set; } // URL a kennel logójához

        [MaxLength(50)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        // Navigációs tulajdonságok:
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Dog> Dogs { get; set; }
        public virtual ICollection<Litter> Litters { get; set; }

        // Tárolni lehet az objektumban a fajtákat, de mivel lekérdezhető, így nem akarom az adatbázisba is betenni
        [NotMapped]
        [Display(Name = "Breed")]
        public IQueryable<Breed> Breeds { get; set; }
    }
}
