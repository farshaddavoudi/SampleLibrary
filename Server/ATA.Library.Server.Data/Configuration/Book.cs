using ATA.Library.Server.Model.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATA.Library.Server.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasUniqueIndexArchivable(b => new { b.CategoryId, b.Title });

            builder
                .HasOne(b => b.Category)
                .WithMany(c => c!.Books)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}