using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Activity> Activities { set; get; }

        public DbSet<ActivityAttendee> ActivityAttendees { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new{ aa.AppUserId, aa.ActivityId}));

            builder.Entity<ActivityAttendee>()
            .HasOne( u => u.AppUser)
            .WithMany( a => a.Activities)
            .HasForeignKey( aa => aa.AppUserId);

            builder.Entity<ActivityAttendee>()
            .HasOne( u => u.Activity)
            .WithMany( a => a.Attendees)
            .HasForeignKey( aa => aa.ActivityId);

        }

    }
}