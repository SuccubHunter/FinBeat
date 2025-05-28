using FinBeat.Application.DTOs;
using FinBeat.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinBeat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
	private readonly ILogger<ItemsController> _logger;
	private readonly IItemService _service;

	public ItemsController(ILogger<ItemsController> logger, IItemService service)
	{
		_logger = logger;
		_service = service;
	}
	
	[HttpPost]
	public async Task<IActionResult> Save([FromBody] List<ItemDto> values)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		await _service.SaveAsync(values);
		return NoContent();
	}

	[HttpGet]
	public async Task<ActionResult<List<ItemDto>>> GetAll()
	{
		var result = await _service.GetAllAsync();
		return Ok(result);
	}
}