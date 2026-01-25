using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(f => f.Rating)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Book)
            .WithMany(b => b.Feedbacks)
            .HasForeignKey(f => f.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
