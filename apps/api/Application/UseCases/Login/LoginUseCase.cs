using Application.Exceptions;
using Application.Utils;
using Domain;
using Infrastructure.Services.Email;
using Infrastructure.Services.JWT;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Login;

public class LoginUseCase(
    ILogger<LoginUseCase> logger, 
    IUnitOfWork unitOfWork,
    JWTService jwt,
    IEmailService emailService,
    IConfiguration configuration
    ) : UseCase<LoginInputDto, LoginOutputDto>(logger), ILoginUseCase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly int EMAIL_LINK_EXPIRATION_IN_MINUTES = 120;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly JWTService _jwt = jwt;
    
    public override async Task<LoginOutputDto> Execute(LoginInputDto input)
    {
        // erro padrao
        _logger.LogInformation("Login");
        var error = new HttpException(400, "Usuário e/ou senha incorreto(s)");
        // pega usuario com email
        var user = await _unitOfWork.User.GetFirstAsync(x => x.Email == input.Email, "ActiveAccountToken");
        if (user == null) throw error;
        // verifica se tem o email verificado
        if (!user.EmailVerified) {
            // verifica se tem token de ativação
            var activeToken = user.ActiveAccountToken;
            // deve criar um token novo
            if (activeToken == null) {
                activeToken = _unitOfWork.ActiveAccountToken.Add(new ActiveAccountToken(
                    Guid.NewGuid().ToString(), 
                    user.Id,
                    DateTime.UtcNow.AddMinutes(EMAIL_LINK_EXPIRATION_IN_MINUTES)
                ));
                if (await _unitOfWork.SaveChangesAsync() == 0) {
                    throw new HttpException(500, "Sua conta não foi ativada ainda, tente mais tarde");
                }
            }

            // envia email com o link de ativação
            var baseUrl = _configuration["ActiveAccountUrl"];
            var url = $"{baseUrl}?token={activeToken.Content}";
            await _emailService.SendEmailAsync(user.Email, "Ativação de conta", url);
            throw new HttpException(400, "Email não verificado, por favor verifique seu email");
        }
        // valida senha
        var correctPass = Security.VerifyPassword(input.Password, user.Password);
        if (!correctPass) throw error;
        
        // cria e retornar token de acesso
        var token = _jwt.GenerateToken(user.Id, user.Email);
        return new LoginOutputDto() { 
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            Expires = token.Expires
        };
    }
}