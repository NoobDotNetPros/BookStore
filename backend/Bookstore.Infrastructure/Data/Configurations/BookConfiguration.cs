using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.ISBN)
            .HasMaxLength(20);

        builder.Property(b => b.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.DiscountPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.Quantity)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasMany(b => b.Feedbacks)
            .WithOne(f => f.Book)
            .HasForeignKey(f => f.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
