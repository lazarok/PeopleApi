using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Entities;

namespace People.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Fullname).HasMaxLength(100);
        builder.Property(x => x.DateOfBirth).HasColumnType("DATE");
        builder.Property(x => x.Email).HasMaxLength(50);
        builder.Property(x => x.PhoneNumber).HasMaxLength(15);
        builder.Property(x => x.Picture).HasMaxLength(256);
        builder.Property(x => x.Dni).HasMaxLength(50);
        builder.Property(x => x.CreatedAt);

        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("Person_Email");
    }
}