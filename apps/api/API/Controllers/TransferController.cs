using API.Dto;
using Application.UseCases.ConfirmFileReceive;
using Application.UseCases.CreateReceiveTransfer;
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
    ICreateReceiveTransfer createReceiveTransfer, 
    IConfirmFileReceive confirmFileReceive, 
    ILogger<TransferController> logger, 
    IUnitOfWork unitOfWork
    ) : BaseController(logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICreateSendTransfer _createSendTransfer = createSendTransfer;
    private readonly ICreateReceiveTransfer _createReceiveTransfer = createReceiveTransfer;
    private readonly IConfirmFileReceive _confirmFileReceive = confirmFileReceive;

    [HttpGet]
    public async Task<IActionResult> GetTransferList([FromQuery] int limit = 20, [FromQuery] int page = 0, [FromQuery] string? type = null)
    {
        var organizationId = GetOrganizationId();
        List<Transfer> transferList = [];
        int count = 0;
        if (type == "send") {
            transferList = await _unitOfWork.Transfer.GetListAsync(
                x => x.OrganizationId == organizationId && x.TransferType == TransferType.Send, 
                limit: limit, 
                offset: page * limit
            );
            count = await _unitOfWork.Transfer.CountAsync(x => x.OrganizationId == organizationId && x.TransferType == TransferType.Send);
        }   
        else if (type == "receive") {
            transferList = await _unitOfWork.Transfer.GetListAsync(
                x => x.OrganizationId == organizationId && x.TransferType == TransferType.Receive, 
                limit: limit, 
                offset: page * limit
            );
            count = await _unitOfWork.Transfer.CountAsync(x => x.OrganizationId == organizationId && x.TransferType == TransferType.Receive);
        }
        else return BadRequest();
        return Ok(TransferGetTransferListDto.Map(transferList, count));
    }

    [HttpGet("{transferKey}")]
    public async Task<IActionResult> GetTransfer([FromRoute] string transferKey)
    {
        var transfer = await _unitOfWork.Transfer.GetByKeyWithFiles(transferKey, 20, 0);
        if (transfer == null) return NotFound();
        return Ok(TransferGetTrasferDto.Map(transfer));
    }

    [HttpPost("receive")]
    public async Task<ActionResult<CreateReceiveTransferOutputDto>> CreateReceiveTransfer([FromBody] CreateReceiveTransferInputDto body, [FromQuery] string? type = null)
    {
        var result = await _createReceiveTransfer.Execute(new() { 
            OrganizationId = GetOrganizationId(),
            AcceptedFiles = body.AcceptedFiles,
            ExpiresAt = body.ExpiresAt,
            MaxFiles = body.MaxFiles,
            MaxSize = body.MaxSize,
            Name = body.Name,
            UserId = GetUserId(),
            Message = body.Message,
            Password = body.Password,
        });
        return Ok(result);
    }
    [HttpPost("send")]
    public async Task<ActionResult<CreateSendTransferOutputDto>> CreateSendTransfer([FromBody] CreateSendTransferInputDto body, [FromQuery] string? type = null)
    {
        var organizationId = GetOrganizationId();
        var result = await _createSendTransfer.Execute(new() { 
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