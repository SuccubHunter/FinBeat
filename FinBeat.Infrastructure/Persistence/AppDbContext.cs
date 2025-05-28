using FinBeat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinBeat.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) {}

	public DbSet<Item> Items => Set<Item>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}