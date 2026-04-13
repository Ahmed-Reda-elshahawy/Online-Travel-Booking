using FluentValidation;
using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using System.Reflection;

namespace OnlineTravelBooking.Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var error = new Error("Validation.Failed", errorMessage);

            // If the response type is non-generic Result
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            // If the response type is Result<T>, call Result.Failure<T>(error) via reflection
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericArg = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m => m.Name == "Failure" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 1);

                if (failureMethod != null)
                {
                    var genericMethod = failureMethod.MakeGenericMethod(genericArg);
                    var result = genericMethod.Invoke(null, new object[] { error });
                    return (TResponse)result!;
                }
            }

            // Fallback: throw to indicate unsupported response type
            throw new InvalidOperationException($"Unable to create a failure result for response type {typeof(TResponse).FullName}");
        }

        return await next();
    }
}
