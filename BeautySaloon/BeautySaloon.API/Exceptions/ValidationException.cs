using FluentValidation.Results;
using System.Text;

namespace BeautySaloon.API.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(ValidationResult validationResult)
        : base(CreateValidationMessage(validationResult))
    {
    }

    private static string CreateValidationMessage(ValidationResult validationResult)
    {
        var messageLines = new List<string>
        {
            "Customer validation failed. Check errors for more details:"
        };

        foreach (var e in validationResult.Errors)
        {
            messageLines.Add($"{e.ErrorMessage}. Attempted value: {e.AttemptedValue}");
        }

        return string.Join(Environment.NewLine, messageLines);
    }
}
