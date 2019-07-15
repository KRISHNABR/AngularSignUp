using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        //Now we can customise this IdentityUser by just adding a column for fullname(i.e by adding a new property)
        [Column(TypeName="nvarchar(150)")]
        public string  FullName { get; set; }
        //Now in order to mention this customisations as part of AuthenticationContext we need to create a property outside 
        //constructor of type DbSet<ApplicationUser>
    }
}
