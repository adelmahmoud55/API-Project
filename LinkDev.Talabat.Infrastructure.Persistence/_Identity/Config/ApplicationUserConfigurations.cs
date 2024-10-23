using LinkDev.Talabat.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identity.Config
{
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser> // hena msh hynf3 a3ml BaseEntity zy StoreContext 3shan el 7 Identity tables mlhomsh BaseEntity wa7d
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           builder.Property(U => U.DisplayName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();



            builder.HasOne(U => U.Address)
                .WithOne(A => A.User)
                .HasForeignKey<Address>(A => A.UserId) // u have to use  .HasForeignKey<Address> cuz in one to one relation , he cannot decide foreignekey entity by himself
                .OnDelete(DeleteBehavior.Cascade);   // once user is deleted the address will be deleted too
        }
    }
}
