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

        public DbSet<Photo> Photo { set; get; }

        public DbSet<Comment> Comments { set; get; }

        public DbSet<UserFollowing> UserFollowings { set; get; }

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

            builder.Entity<Comment>()
            .HasOne( a => a.Activity)
            .WithMany( c => c.Comments)
            .OnDelete( DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>( b => 
            {
                b.HasKey( k => new { k.ObserverId, k.TargetId});

                b.HasOne( o => o.Observer)
                 .WithMany(f => f.Followings)
                 .HasForeignKey( o => o.ObserverId)
                 .OnDelete(DeleteBehavior.Cascade);

                
                b.HasOne( o => o.Target)
                 .WithMany(f => f.Followers)
                 .HasForeignKey( o => o.TargetId)
                 .OnDelete(DeleteBehavior.Cascade);
            });



        }

    }
}