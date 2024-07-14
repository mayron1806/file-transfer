namespace Application.UseCases;

public interface IUseCase<Input, Output>
{
    Task<Output> Execute(Input input);
}