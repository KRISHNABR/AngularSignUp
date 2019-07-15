using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Identity Core works based on EntityFramework Core Code First Approach. So inside this assembly there is an entity type for
//user which is called Identity User. Inside that we have a column properties for saving details like username, email, encrypted
//passwords etc.If we want to add a new property or  column or customise an existing column. We have to inherit that class identity User
//identity User into this project. For that we will add a new class in Model ApplicationUser:IdentityUSer

namespace WebAPI.Models
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options) //In this options parameter we need to pass values like provider(SQL server, dbconstring)
            //Where do we pass these values? For that we make use of dependency injcetion in ASP.NET core. Its done using configureServices() method in startup.cs
        {
                
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
