using Application.Exceptions;
using Domain;
using Infrastructure.Services.Email;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ForgetPassword
{
    public class ForgetPasswordUseCase(ILogger<ForgetPasswordUseCase> logger, IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration) : UseCase<ForgetPasswordInputDto, bool>(logger), IForgetPasswordUseCase
    {
        private readonly int EMAIL_LINK_EXPIRATION_IN_MINUTES = 120;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmailService _emailService = emailService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public override async Task<bool> Execute(ForgetPasswordInputDto input)
        {
            // verificar se o email existe
            var userWithEmail = await _unitOfWork.User.GetFirstAsync(x => x.Email == input.Email);
            if (userWithEmail == null) throw new HttpException(400, "Email nao encontrado");
            // criar token
            var token = _unitOfWork.ResetPasswordToken.Add(new ResetPasswordToken(
                Guid.NewGuid().ToString(), 
                userWithEmail.Id,
                DateTime.UtcNow.AddMinutes(EMAIL_LINK_EXPIRATION_IN_MINUTES)
            ));
            if (await _unitOfWork.SaveChangesAsync() == 0) {
                throw new HttpException(500, "Ocorreu um erro ao tentar redefinir a senha, tente mais tarde");
            }
            
            // envia email com o link de redefinição de senha
            var baseUrl = _configuration["ResetPasswordUrl"];
            var url = $"{baseUrl}?token={token.Content}";
            await _emailService.SendEmailAsync(userWithEmail.Email, "Redefinição de senha", url);

            return true;
        }
    }
}