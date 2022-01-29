using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Data.Configuration
{
	public class CourtEntityTypeConfiguration : IEntityTypeConfiguration<Court>
	{
		public void Configure(EntityTypeBuilder<Court> builder)
		{
			builder.ToTable("Court").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new CourtId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.Capacity);
		}
	}
}
