using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class VenueEntityTypeConfiguration : IEntityTypeConfiguration<Venue>
	{
		public void Configure(EntityTypeBuilder<Venue> builder)
		{
			builder.ToTable("Venue").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new VenueId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.Surface);
			builder.HasMany(b => b.Courts).WithOne().IsRequired()
				.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
