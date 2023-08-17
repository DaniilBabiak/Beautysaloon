using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using PhoneNumbers;
using System.Globalization;
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
        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var regions = phoneNumberUtil.GetSupportedRegions();

        foreach (var region in regions)
        {
            PhoneNumber number = phoneNumberUtil.Parse(phoneNumber, region);

            var isValid = phoneNumberUtil.IsValidNumberForRegion(number, region);

            if (isValid)
            {
                return true;
            }
        }

        return false;
    }
}