using FluentValidation.Results;

namespace BeautySaloon.API.Exceptions;

public class CustomerNotValidException : ValidationException
{
    public CustomerNotValidException(ValidationResult validationResult) : base(validationResult)
    {
    }
}
