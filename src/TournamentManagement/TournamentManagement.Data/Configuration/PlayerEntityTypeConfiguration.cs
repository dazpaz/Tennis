using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
	{
		public void Configure(EntityTypeBuilder<Player> builder)
		{
			builder.ToTable("Player").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new PlayerId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.SinglesRank);
			builder.Property(p => p.DoublesRank);
			builder.Property(p => p.Gender);
		}
	}
}
