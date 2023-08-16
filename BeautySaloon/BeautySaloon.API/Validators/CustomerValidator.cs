using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BeautySaloon.API.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

        RuleFor(customer => customer.PhoneNumber)
            .Must(BeAValidPhoneNumber)
            .When(customer => !string.IsNullOrEmpty(customer.PhoneNumber))
            .WithMessage("Invalid phone number");
    }

    private bool BeAValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return true;

        // Регулярное выражение для проверки номера телефона в формате (XXX) XXX-XXXX
        string pattern = @"^\(\d{3}\) \d{3}-\d{4}$";

        return Regex.IsMatch(phoneNumber, pattern);
    }
}