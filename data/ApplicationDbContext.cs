namespace data.ApplicationDbContext;

using Microsoft.EntityFrameworkCore;
using Reviews.Models;

public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options): base(options){}

    public DbSet<Review> Reviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>(entity =>
        {

            entity.ToTable(tb =>
            {
            tb.HasCheckConstraint("CK_Review_Rating", "Rating >= 1 AND Rating <= 10");
            });

            entity.ToTable("Reviews");

            entity.HasKey(r => r.ReviewId);
            entity.Property(r => r.ReviewId)
                  .ValueGeneratedOnAdd();

            entity.Property(r => r.UserId)
                  .IsRequired()
                  .HasMaxLength(450);

            entity.Property(r => r.MovieId)
                  .IsRequired();

            entity.Property(r => r.Text)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(r => r.Rating)
                  .HasColumnType("int")
                  .IsRequired(true);

            entity.Property(r => r.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
