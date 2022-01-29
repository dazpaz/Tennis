using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.RoundAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class RoundEntityTypeConfiguration : IEntityTypeConfiguration<Round>
	{
		public void Configure(EntityTypeBuilder<Round> builder)
		{
			builder.ToTable("Round").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new RoundId(p));
			builder.HasOne(p => p.Tournament).WithMany().IsRequired();
			builder.Property(p => p.EventType);
			builder.Property(p => p.RoundNumber).IsRequired();
			builder.Property(p => p.CompetitorCount).IsRequired();
			builder.Ignore(p => p.Title);
		}
	}
}
