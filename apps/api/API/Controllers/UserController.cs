using API.Dto;
using Application.Exceptions;
using Application.UseCases.ActiveAccount;
using Application.UseCases.CreateAccount;
using Application.UseCases.ForgetPassword;
using Application.UseCases.Login;
using Application.UseCases.ResetPassword;
using Infrastructure.Services.JWT;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    ILogger<UserController> logger, 
    IUnitOfWork unitOfWork, 
    ICreateAccountUseCase createAccountUseCase,
    IActiveAccountUseCase activeAccountUseCase,
    IForgetPasswordUseCase forgetPasswordUseCase,
    IResetPasswordUseCase resetPasswordUseCase,
    JWTService jwt,
    ILoginUseCase loginUseCase) : BaseController(logger)
{
    private readonly JWTService _jwt = jwt;
    private readonly IActiveAccountUseCase _activeAccountUseCase = activeAccountUseCase;
    private readonly IForgetPasswordUseCase _forgetPasswordUseCase = forgetPasswordUseCase;
    private readonly IResetPasswordUseCase _resetPasswordUseCase = resetPasswordUseCase;
    private readonly ILoginUseCase _loginUseCase = loginUseCase;
    private readonly ICreateAccountUseCase _createAccountUseCase = createAccountUseCase;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetUserId();
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        return Ok(user);
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginInputDto body) => Ok(await _loginUseCase.Execute(body));

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountInputDto body) => Ok(await _createAccountUseCase.Execute(body));

    [HttpPost("active-account")]
    public async Task<IActionResult> ActiveAccount([FromBody] ActiveAccountInputDto body) => Ok(await _activeAccountUseCase.Execute(body));

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordInputDto body) => Ok(await _forgetPasswordUseCase.Execute(body));

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputDto body) => Ok(await _resetPasswordUseCase.Execute(body));
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenBody body)
    {
        if(body.RefreshToken == null) throw new HttpException(400, "Token invalido");
        var content = _jwt.DecodeToken(body.RefreshToken);
        if(content == null) throw new HttpException(400, "Token invalido");
        var userId = int.Parse(content);
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        if(user == null) throw new HttpException(400, "Token invalido");
        var token = _jwt.GenerateToken(userId, user.Email);
        return Ok(token);
    }
}