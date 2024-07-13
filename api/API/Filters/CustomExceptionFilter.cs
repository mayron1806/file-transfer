using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class CustomExceptionFilter(ILogger<CustomExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            HttpException httpException => new ObjectResult(new { message = httpException.Message }) { StatusCode = (int)httpException.StatusCode },
            _ => new ObjectResult(new { message = "Ocorreu um erro inesperado" }) { StatusCode = 500 },
        };
        _logger.LogError(context.Exception, context.Exception.Message);
    }
}