using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.CompetitorAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class CompetitorEntityTypeConfiguration : IEntityTypeConfiguration<Competitor>
	{
		public void Configure(EntityTypeBuilder<Competitor> builder)
		{
			builder.ToTable("Competitor").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new CompetitorId(p));
			builder.Property(p => p.EventType);
			builder.Property(p => p.Seeding)
				.HasConversion(p => p.Seed, p => new Seeding(p));
			builder.HasOne(p => p.Tournament).WithMany().IsRequired();
			builder.Property(p => p.PlayerOneName)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.PlayerTwoName)
				.HasMaxLength(50);
		}
	}
}
