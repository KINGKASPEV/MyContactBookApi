using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Model
{
    public class AppUser : IdentityUser
    {
        public Contact Contact { get; set; }
    }
}
