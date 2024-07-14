using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/organization/{organizationId}/transfer/{transferKey}/[controller]")]
public class FileController(
    ILogger<FileController> logger,
    IUnitOfWork unitOfWork
): BaseController(logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    [HttpGet]
    public async Task<IActionResult> GetTransferFiles([FromRoute] string transferKey, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        var files = await _unitOfWork.File.GetListAsync(x => x.Transfer!.Key == transferKey, limit: limit, offset: offset);
        if (files == null) return NotFound();
        return Ok(files);
    }
}
