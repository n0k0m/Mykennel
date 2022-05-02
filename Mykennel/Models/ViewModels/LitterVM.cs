using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models.ViewModels
{
    public class LitterVM
    {
        [Display(Name = "Bookable Litters")]
        public IQueryable<Litter> AvailableLitters { get; set; }
        [Display(Name = "Planned Litters")]
        public IQueryable<Litter> PlannedLitters { get; set; }
    }
}
