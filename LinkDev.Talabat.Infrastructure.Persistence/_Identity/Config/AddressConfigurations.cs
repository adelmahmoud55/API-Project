using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence._Comman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identity.Config
{
    [DbContextType(typeof(StoreIdentityDbContext))] // this for the reflection to get the type of the DbContext
    internal class AddressConfigurations : IEntityTypeConfiguration<Address>
    {

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(nameof(Address.Id)).ValueGeneratedOnAdd();
            builder.Property(nameof(Address.FirstName)).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(nameof(Address.LastName)).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(nameof(Address.Street)).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(nameof(Address.City)).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(nameof(Address.Country)).HasColumnType("varchar").HasMaxLength(50);






            builder.ToTable("Addresses");
        }
    }
}
