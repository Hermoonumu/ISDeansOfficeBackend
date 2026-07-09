using DeanInfoSystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.Application.Common.Handlers;




public class GlobalExceptionHandler(IProblemDetailsService prDeService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                            Exception exception,
                                            CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            DepartmentDoesntExistException => StatusCodes.Status404NotFound,
            EnrollmentException => StatusCodes.Status409Conflict,
            GradeDoesntExistException => StatusCodes.Status404NotFound,
            PasswordCheckFailedException => StatusCodes.Status401Unauthorized,
            PositionException => StatusCodes.Status422UnprocessableEntity,
            ProgramDoesntExistException => StatusCodes.Status404NotFound,
            RefreshFailedException => StatusCodes.Status401Unauthorized,
            SubjectAlreadyExistsException => StatusCodes.Status409Conflict,
            SubjectDoesntExistException => StatusCodes.Status404NotFound,
            UpdateFailedException => StatusCodes.Status400BadRequest,
            UserAlreadyExistsException => StatusCodes.Status409Conflict,
            UserDoesntExistException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };


        return await prDeService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Type = exception.GetType().Name,
                Title = "An error has occurred",
                Detail = exception.Message
            }
        });



    }
}