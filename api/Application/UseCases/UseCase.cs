using Microsoft.Extensions.Logging;

namespace Application.UseCases;

public abstract class UseCase<Input, Output>(ILogger<UseCase<Input, Output>> logger): IUseCase<Input, Output>
{
    protected readonly ILogger<UseCase<Input, Output>> _logger = logger;
    public abstract Task<Output> Execute(Input input);
}