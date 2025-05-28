using FinBeat.Core.Entities;

namespace FinBeat.Core.Interfaces;

public interface IItemRepository
{
	Task ClearAsync();
	Task BulkInsertAsync(IEnumerable<Item> entities);
	Task<List<Item>> GetAllAsync();
}