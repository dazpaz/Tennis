using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class EventEntryEntityTypeConfiguration : IEntityTypeConfiguration<EventEntry>
	{
		public void Configure(EntityTypeBuilder<EventEntry> builder)
		{
			builder.ToTable("EventEntry").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new EventEntryId(p));
			builder.Property(p => p.EventType);
			builder.Property(p => p.Rank);

			builder.HasOne(p => p.PlayerOne).WithMany().IsRequired();
			builder.HasOne(p => p.PlayerTwo).WithMany();
		}
	}
}
