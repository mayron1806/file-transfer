using Application.Exceptions;
using Application.Utils;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ResetPassword;

public class ResetPasswordUseCase(ILogger<ResetPasswordUseCase> logger, IUnitOfWork unitOfWork) : UseCase<ResetPasswordInputDto, bool>(logger), IResetPasswordUseCase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public override async Task<bool> Execute(ResetPasswordInputDto input)
    {
        var token = await _unitOfWork.ResetPasswordToken.GetFirstAsync(x => x.Content == input.Token, "User");
        if (token == null) throw new HttpException(400, "Token invalido ou expirado");
        var user = token.User;
        if (user == null) throw new HttpException(400, "Token invalido");
        if (DateTime.UtcNow > token.ExpiresAt) {
            throw new HttpException(400, "Token expirado");
        }
        user.UpdatePassword(Security.HashPassword(input.Password));
        
        _unitOfWork.User.Update(user);
        _unitOfWork.ResetPasswordToken.Delete(token);
        if (await _unitOfWork.SaveChangesAsync() == 0) {
            throw new HttpException(500, "Ocorreu um erro ao tentar redefinir a senha, tente mais tarde");
        }
        
        return true;
    }
}