using ContactBookAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBookAPI.Data
{
    public class ContactBookDbContext : IdentityDbContext<AppUser>
    {
        public ContactBookDbContext(DbContextOptions<ContactBookDbContext> options)
            : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        //public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Contact)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Contact>(c => c.UserId);
        }
    }
}
