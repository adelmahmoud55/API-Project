using LinkDev.Talabat.Core.Domain.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identity
{
    internal class StoreIdentityDbContext : IdentityDbContext<ApplicationUser> // we used this overload cuz we only want to decide the type of user we want to use
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //we have to override and call the base method to keep the default behavior of the IdentityDbContext, 3shan ana b inherit DbSets mn IdentityDbContext "7" 
            base.OnModelCreating(builder); 


            /*builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly); */// msh hynf3 ast5dmha 3shan hyr9g y geb kol el classes ely b implement IEntityTypeConfiguration wna 3ayz bs bto3 IdentityContext msh m7tag bto3 el storeContext


            // Apply them manually
            builder.ApplyConfiguration(new ApplicationUserConfigurations()); 
            builder.ApplyConfiguration(new AddressConfigurations()); 

        }



        // no neeed to create DbSet of address EF core will generate the table as address involved in one to one relation with ApplicationUser 
        // to make any other config for the Address use config class and apply it in the OnModelCreating method thr
    }
    
   
    
}
