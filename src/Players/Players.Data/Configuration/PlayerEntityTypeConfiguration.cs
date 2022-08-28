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
			.HasConversion(p => p.Name, p => (PlayerName)p)
			.HasMaxLength(PlayerName.MaxNameLength)
			.IsRequired();
		builder.Property(p => p.LastName)
			.HasConversion(p => p.Name, p => (PlayerName)p)
			.HasMaxLength(PlayerName.MaxNameLength)
			.IsRequired();
		builder.Property(p => p.Email)
			.HasConversion(p => p.Email, p => (EmailAddress)p)
			.HasMaxLength(EmailAddress.MaxEmailLength)
			.IsRequired();
		builder.Property(p => p.Gender);
		builder.Property(p => p.DateOfBirth);
		builder.Property(p => p.Plays);
		builder.Property(p => p.Height)
			.HasConversion(p => p.Value, p => (Height)p)
			.IsRequired();
		builder.HasOne(p => p.Country).WithMany().IsRequired();
		builder.Property(p => p.SinglesRank)
			.HasConversion(p => p.Rank, p => (Ranking)p)
			.IsRequired();
		builder.Property(p => p.DoublesRank)
			.HasConversion(p => p.Rank, p => (Ranking)p)
			.IsRequired();
		builder.Property(p => p.SinglesRankingPoints)
			.HasConversion(p => p.Points, p => (RankingPoints)p)
			.IsRequired();
		builder.Property(p => p.DoublesRankingPoints)
			.HasConversion(p => p.Points, p => (RankingPoints)p)
			.IsRequired();
	}
}
