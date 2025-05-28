using FinBeat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinBeat.Infrastructure.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
	public void Configure(EntityTypeBuilder<Item> builder)
	{
		builder.HasKey(a => a.Id);
		builder.HasIndex(a => a.Id);
		builder.HasIndex(a => a.Code);
		builder.HasIndex(a => a.Value);
	}
}