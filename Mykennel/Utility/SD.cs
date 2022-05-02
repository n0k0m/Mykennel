using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Utility
{
    // Statikus definíciókat használtam, mivel az AspNetRoles Role-ok beállításához
    public static class SD
    {
        public const string Role_User_Indi = "Individual";
        public const string Role_User_Breeder = "Breeder";
        public const string Role_Admin = "Admin";
    }
}
