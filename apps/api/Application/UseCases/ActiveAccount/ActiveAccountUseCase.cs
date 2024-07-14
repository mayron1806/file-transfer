using Application.Exceptions;
using Domain;
using Infrastructure.Services.Email;
using Infrastructure.Services.JWT;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ActiveAccount
{
    public class ActiveAccountUseCase(
        ILogger<ActiveAccountUseCase> logger, 
        IUnitOfWork unitOfWork, 
        JWTService jwt,
        IEmailService emailService,
        IConfiguration configuration
        ) : UseCase<ActiveAccountInputDto, ActiveAccountOutputDto>(logger), IActiveAccountUseCase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly int EMAIL_LINK_EXPIRATION_IN_MINUTES = 120;
        private readonly IEmailService _emailService = emailService;
        private readonly JWTService _jwt = jwt;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public override async Task<ActiveAccountOutputDto> Execute(ActiveAccountInputDto input)
        {
            var token = await _unitOfWork.ActiveAccountToken.GetFirstAsync(x => x.Content == input.Token, "User");
            if (token == null) throw new HttpException(400, "Token invalido ou expirado");
            var user = token.User;
            if (user == null) throw new HttpException(400, "Token invalido");
            if (user.EmailVerified) throw new HttpException(400, "Email ja verificado");
            if (DateTime.UtcNow > token.ExpiresAt) {
                var activeToken = _unitOfWork.ActiveAccountToken.Add(new ActiveAccountToken(
                    Guid.NewGuid().ToString(), 
                    user.Id,
                    DateTime.UtcNow.AddMinutes(EMAIL_LINK_EXPIRATION_IN_MINUTES)
                ));
                await _unitOfWork.SaveChangesAsync();
                // envia email com o link de ativação
                var baseUrl = _configuration["ActiveAccountUrl"];
                var url = $"{baseUrl}?token={activeToken.Content}";
                await _emailService.SendEmailAsync(user.Email, "Ativação de conta", url);
                throw new HttpException(400, "O link de ativação expirou, enviamos um novo link para seu email");
            }
            user.VerifyEmail();
            
            _unitOfWork.User.Update(user);
            _unitOfWork.ActiveAccountToken.Delete(token);
            
            await _unitOfWork.SaveChangesAsync();
            var jwt = _jwt.GenerateToken(user.Id, user.Email);
            return new ActiveAccountOutputDto()
            {
                AccessToken = jwt.AccessToken,
                RefreshToken = jwt.RefreshToken,
                Expires = jwt.Expires
            };
        }
    }
}