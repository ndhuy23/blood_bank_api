using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using BloodBank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.DataAccess
{
    public class BloodBankContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Donor> Donors { get; set; }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<Blood> Bloods { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<RequestBlood> RequestBloods { get; set;}

        public DbSet<SessionDonor> SessionDonors { get; set; }

        public DbSet<History> Histories { get; set; }

        public BloodBankContext(DbContextOptions<BloodBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .HasMany(ac => ac.SessionDonors)
                .WithOne(sd => sd.Activity)
                .HasForeignKey(sd => sd.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hospital>()
                        .HasMany(h => h.Activities)
                        .WithOne(ac => ac.Hospital)
                        .HasForeignKey(ac => ac.HospitalId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hospital>()
                        .HasMany(h => h.Bloods)
                        .WithOne(bl => bl.Hospital)
                        .HasForeignKey(bl => bl.HospitalId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hospital>()
                        .HasMany(h => h.RequestBloods)
                        .WithOne(rq => rq.Hospital)
                        .HasForeignKey(rq => rq.HospitalId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Donor>()
                        .HasMany(d => d.Histories)
                        .WithOne(h => h.Donor)
                        .HasForeignKey(h => h.DonorId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Donor>()
                        .HasMany(d => d.SessionDonors)
                        .WithOne(sd => sd.Donor)
                        .HasForeignKey(sd => sd.DonorId)
                        .OnDelete(DeleteBehavior.Restrict); // Thay đổi hành động cascade

            modelBuilder.Entity<Activity>()
                        .HasIndex(a => a.DateActivity);

            modelBuilder.Entity<Hospital>()
                        .HasOne(h => h.Account)
                        .WithOne(a => a.Hospital)
                        .HasForeignKey<Hospital>(h => h.AccountId);

            modelBuilder.Entity<Donor>()
                        .HasOne(d => d.Account)
                        .WithOne(a => a.Donor)
                        .HasForeignKey<Donor>(d => d.AccountId);
        }
        public override int SaveChanges()
        {
            UpdateIsDelete();
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateIsDelete();
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateIsDelete()
        {
            var entities = ChangeTracker.Entries<ISoftDelete>().Where(x => x.State == EntityState.Deleted);
            
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity.GetType().GetProperty("IsDelete") != null)
                    {
                        entity.Entity.IsDelete = true;
                        entity.Entity.DeleteDate = DateTime.UtcNow;
                    }

                }
            }
        }
        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries<EntityAuditBase<Guid>>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity.GetType().GetProperty("CreatedDate") != null)
                    {
                        entity.Entity.CreatedDate = DateTime.UtcNow;
                    }
                    entity.Entity.ModifiedDate = DateTime.UtcNow;
                }


            }
        }
    }
}
