using FinBeat.Application.DTOs;
using FinBeat.Application.Interfaces;
using FinBeat.Core.Entities;
using FinBeat.Core.Interfaces;

namespace FinBeat.Application.Services;

public interface IItemService
{
	Task SaveAsync(IEnumerable<ItemDto> values);
	Task<List<ItemDto>> GetAllAsync();
}

public class ItemService : IItemService
{
	private readonly IItemRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public ItemService(IItemRepository repository, IUnitOfWork unitOfWork)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task SaveAsync(IEnumerable<ItemDto> values)
	{
		var sorted = values
			.OrderBy(v => v.Code)
			.Select(v => new Item()
			{
				Code = v.Code,
				Value = v.Value
			})
			.ToList();

		await _repository.ClearAsync();
		await _repository.BulkInsertAsync(sorted);
		await _unitOfWork.SaveChangesAsync();
	}

	public async Task<List<ItemDto>> GetAllAsync()
	{
		var data = await _repository.GetAllAsync();

		return data.Select(x => new ItemDto()
		{
			Code = x.Code,
			Value = x.Value
		}).ToList();
	}
}