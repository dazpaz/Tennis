using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Players.Domain.CountryAggregate;

namespace Players.Data.Configuration
{
	internal class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.ToTable("Country").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new CountryId(p));
			builder.Property(p => p.ShortName)
				.IsRequired()
				.HasMaxLength(3);
			builder.Property(p => p.FullName)
				.IsRequired()
				.HasMaxLength(50);
		}
	}
}
