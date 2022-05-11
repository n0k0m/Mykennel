using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Kennel Kennel { get; set; }
    }
}
