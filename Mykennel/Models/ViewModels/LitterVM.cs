using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models.ViewModels
{
    public class LitterVM
    {
        public Litter Litter { get; set; }
        public Kennel Kennel { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "Breed")]
        public Breed Breed { get; set; }
        [Display(Name = "Country")]
        public Country Country { get; set; }
    }
}
