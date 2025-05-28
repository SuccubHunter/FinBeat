using FinBeat.Core.Entities;
using FinBeat.Core.Interfaces;
using FinBeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinBeat.Infrastructure.Repositories;

public class ItemRepository: IItemRepository
{
	private readonly AppDbContext _context;

	public ItemRepository(AppDbContext context)
	{
		_context = context;
	}
	
	public Task ClearAsync()
	{
		_context.Items.RemoveRange(_context.Items);
		return Task.CompletedTask;
	}

	public async Task BulkInsertAsync(IEnumerable<Item> entities)
	{
		await _context.Items.AddRangeAsync(entities);
	}

	public async Task<List<Item>> GetAllAsync()
	{
		return await _context.Items.OrderBy(x => x.Code).ToListAsync();
	}
}