using Application.Exceptions;
using Application.Utils;
using Domain;
using Infrastructure.Services.Email;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.UseCases.CreateAccount
{
    public class CreateAccountUseCase(
        ILogger<CreateAccountUseCase> logger, 
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IConfiguration configuration
        ) : UseCase<CreateAccountInputDto, bool>(logger), ICreateAccountUseCase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly int EMAIL_LINK_EXPIRATION_IN_MINUTES = 120;
        private readonly IEmailService _emailService = emailService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public override async Task<bool> Execute(CreateAccountInputDto input)
        {
            // verificar se o email existe
            var userWithEmail = await _unitOfWork.User.GetFirstAsync(x => x.Email == input.Email);
            if (userWithEmail != null) throw new HttpException(400, "Email ja existe");

            // criptografar a senha
            string hashedPassword = Security.HashPassword(input.Password);
            
            // cria conta
            var user = new User(input.Name, input.Email, hashedPassword);
            user.AddActiveAccountToken(new ActiveAccountToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(EMAIL_LINK_EXPIRATION_IN_MINUTES)
            ));
            _unitOfWork.User.Add(user);
            if (await _unitOfWork.SaveChangesAsync() == 0) {
                _logger.LogError("Erro ao criar conta");
                throw new HttpException(500, "Ocorreu um erro ao tentar cadastra sua conta, tente novamente mais tarde.");
            }

            _logger.LogInformation(JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
            
            // criar url
            var baseUrl = _configuration["ActiveAccountUrl"];
            var url = $"{baseUrl}?token={user.ActiveAccountToken!.Content}";
            _logger.LogInformation(url);
            // enviar email com url de ativação
            await _emailService.SendEmailAsync(user.Email, "Ativação de conta", url);

            return true;
        }
    }
}