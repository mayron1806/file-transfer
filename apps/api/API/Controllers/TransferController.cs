using API.Dto;
using Application.Exceptions;
using Application.UseCases.ConfirmFileReceive;
using Application.UseCases.CreateSendTransfer;
using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/organization/{organizationId}/[controller]")]
[Authorize]
public class TransferController(
    ICreateSendTransfer createSendTransfer, 
    IConfirmFileReceive confirmFileReceive, 
    ILogger<TransferController> logger, 
    IUnitOfWork unitOfWork
    ) : BaseController(logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICreateSendTransfer _CreateSendTransfer = createSendTransfer;
    private readonly IConfirmFileReceive _confirmFileReceive = confirmFileReceive;

    [HttpGet]
    public async Task<IActionResult> GetTransferList([FromQuery] int limit = 20, [FromQuery] int offset = 0, [FromQuery] string? type = null)
    {
        var organizationId = GetOrganizationId();
        List<Transfer> transferList = [];
        if (type == "send") {
            transferList = await _unitOfWork.Transfer.GetListAsync(
                x => x.OrganizationId == organizationId && x.TransferType == TransferType.Send, 
                limit: limit, 
                offset: offset
            );
        }
        else if (type == "receive") {
            transferList = await _unitOfWork.Transfer.GetListAsync(
                x => x.OrganizationId == organizationId && x.TransferType == TransferType.Receive, 
                limit: limit, 
                offset: offset
            );
        }
        else return BadRequest();
        return Ok(TransferGetTransferListDto.Map(transferList));
    }

    [HttpGet("{transferKey}")]
    public async Task<IActionResult> GetTransfer([FromRoute] string transferKey)
    {
        var transfer = await _unitOfWork.Transfer.GetByKeyWithFiles(transferKey, 20, 0);
        if (transfer == null) return NotFound();
        return Ok(TransferGetTrasferDto.Map(transfer));
    }

    [HttpPost]
    public async Task<ActionResult<CreateSendTransferInputDto>> CreateTransfer([FromBody] CreateSendTransferInputDto body, [FromQuery] string? type = null)
    {
        var organizationId = GetOrganizationId();
        var result = await _CreateSendTransfer.Execute(new() { 
            OrganizationId = organizationId,
            Files = body.Files,
            EmailsDestination = body.EmailsDestination,
            ExpiresAt = body.ExpiresAt,
            ExpiresOnDownload = body.ExpiresOnDownload,
            Message = body.Message,
            Password = body.Password,
        });
        return Ok(result);
    }
    [HttpPost("confirm-receive")]
    public async Task<IActionResult> ConfirmReceive([FromBody] ConfirmFileReceiveInputDto body) => Ok(await _confirmFileReceive.Execute(body));
}