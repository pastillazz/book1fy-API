using System.Reflection;
using Domain.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            validators.Select(validator =>
                validator.ValidateAsync(context, cancellationToken)));

        Error[] errors = results
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error =>
                new Error(
                    error.PropertyName, error.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }
        
        return await next(cancellationToken);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
    where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }
        Type valueType = typeof(TResult).GetGenericArguments()[0];
        
        Type validationResultType=typeof(ValidationResult<>)
            .MakeGenericType(valueType);
        
        MethodInfo method=validationResultType
            .GetMethod(nameof(ValidationResult.WithErrors),
                BindingFlags.Public | BindingFlags.Static)!;

        return (TResult)method.Invoke(null, [errors])!;
    }
}