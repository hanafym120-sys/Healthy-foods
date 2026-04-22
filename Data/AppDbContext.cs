using HealthyBites.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyBites.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<SugarReading> SugarReadings { get; set; } = null!;
        public DbSet<Prediction> Predictions { get; set; } = null!;
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<UserRestaurant> UserRestaurants { get; set; } = null!;
        public DbSet<RestaurantReview> RestaurantReviews { get; set; } = null!;
        public DbSet<DailyCalory> DailyCalories { get; set; } = null!;
        public DbSet<MealBreakdown> MealBreakdowns { get; set; } = null!;
        public DbSet<Disease> Diseases { get; set; } = null!;
        public DbSet<UserDisease> UserDiseases { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Prediction>()
                .HasOne(p => p.SugarReading)
                .WithOne(s => s.Prediction)
                .HasForeignKey<Prediction>(p => p.SugarReadingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SugarReading>()
                .HasOne(s => s.User)
                .WithMany(u => u.SugarReadings)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prediction>()
                .HasOne(p => p.User)
                .WithMany(u => u.Predictions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRestaurant>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRestaurants)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRestaurant>()
                .HasOne(ur => ur.Restaurant)
                .WithMany(r => r.UserRestaurants)
                .HasForeignKey(ur => ur.RestaurantId);

            modelBuilder.Entity<RestaurantReview>()
                .HasOne(rr => rr.User)
                .WithMany(u => u.RestaurantReviews)
                .HasForeignKey(rr => rr.UserId);

            modelBuilder.Entity<RestaurantReview>()
                .HasOne(rr => rr.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(rr => rr.RestaurantId);

            modelBuilder.Entity<DailyCalory>()
                .HasOne(d => d.User)
                .WithMany(u => u.DailyCaloryEntries)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<UserDisease>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDiseases)
                .HasForeignKey(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDisease>()
                .HasOne(ud => ud.Disease)
                .WithMany(d => d.UserDiseases)
                .HasForeignKey(ud => ud.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserDisease>()
                .HasIndex(ud => new { ud.UserId, ud.DiseaseId })
                .IsUnique();

            modelBuilder.Entity<Meal>()
                .Property(m => m.DiseaseTagsJson)
                .HasColumnName("DiseaseTags");
        }
    }
}
