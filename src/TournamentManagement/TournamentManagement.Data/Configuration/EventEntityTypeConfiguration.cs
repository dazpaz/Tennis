using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
	{
		public void Configure(EntityTypeBuilder<Event> builder)
		{
			builder.ToTable("Event").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new EventId(p));
			builder.Property(p => p.EventType);
			builder.Property(p => p.IsCompleted);
			builder.Ignore(p => p.SinglesEvent);

			builder.OwnsOne(p => p.EventSize, p =>
			{
				p.Property(pp => pp.EntrantsLimit).IsRequired().HasColumnName("EntrantsLimit");
				p.Property(pp => pp.NumberOfSeeds).IsRequired().HasColumnName("NumberOfSeeds");
			}).Navigation(p => p.EventSize).IsRequired();

			builder.OwnsOne(p => p.MatchFormat, p =>
			{
				p.Property(pp => pp.NumberOfSets).IsRequired().HasColumnName("NumberOfSets");
				p.Property(pp => pp.FinalSetType).IsRequired().HasColumnName("FinalSetType");
			}).Navigation(p => p.MatchFormat).IsRequired();

			builder.HasMany(b => b.Entries).WithOne().IsRequired()
				.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
