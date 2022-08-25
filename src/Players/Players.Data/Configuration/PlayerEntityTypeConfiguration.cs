using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Players.Domain.PlayerAggregate;

namespace Players.Data.Configuration;

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
	public void Configure(EntityTypeBuilder<Player> builder)
	{
		builder.ToTable("Player").HasKey(k => k.Id);
		builder.Property(p => p.Id)
			.HasConversion(p => p.Id, p => new PlayerId(p));
		builder.Property(p => p.FirstName)
			.IsRequired()
			.HasMaxLength(50);
		builder.Property(p => p.LastName)
			.IsRequired()
			.HasMaxLength(50);
		builder.Property(p => p.Gender);
		builder.Property(p => p.DateOfBirth);
		builder.Property(p => p.Plays);
		builder.Property(p => p.Height);
		builder.Property(p => p.Country)
			.IsRequired()
			.HasMaxLength(50);
		builder.Property(p => p.SinglesRank);
		builder.Property(p => p.DoublesRank);
		builder.Property(p => p.SinglesRankingPoints);
		builder.Property(p => p.DoublesRankingPoints);
	}
}
