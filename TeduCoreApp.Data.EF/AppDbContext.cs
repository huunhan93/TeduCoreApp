using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.EF.Extensions;
using TeduCoreApp.Data.EF.Configurations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TeduCoreApp.Data.Interfaces;

namespace TeduCoreApp.Data.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options)
        {

        }
        
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity Config
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins")
                .HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens")
                .HasKey(x => x.UserId);

            #endregion

            builder.AddConfiguration(new TagConfiguration());

            base.OnModelCreating(builder);
            
        }

        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as IDateTracking;
                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
    
}
