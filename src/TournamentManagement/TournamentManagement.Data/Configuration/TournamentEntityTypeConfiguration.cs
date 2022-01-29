using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class TournamentEntityTypeConfiguration : IEntityTypeConfiguration<Tournament>
	{
		public void Configure(EntityTypeBuilder<Tournament> builder)
		{
			builder.ToTable("Tournament").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new TournamentId(p));
			builder.Property(p => p.Title)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.State);
			builder.Property(p => p.Level);
			builder.OwnsOne(p => p.Dates, p =>
			{
				p.Property(pp => pp.StartDate).IsRequired().HasColumnName("StartDate");
				p.Property(pp => pp.EndDate).IsRequired().HasColumnName("EndDate");
			}).Navigation(p => p.Dates).IsRequired();

			builder.HasOne(p => p.Venue).WithMany().IsRequired();

			builder.HasMany(b => b.Events).WithOne().IsRequired()
				.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
