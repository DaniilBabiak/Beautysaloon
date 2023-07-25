using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using FluentValidation;

namespace BeautySaloon.API.Validators;

public class CommentValidator : AbstractValidator<Comment>
{
    public CommentValidator()
    {
        RuleFor(comment => comment.Rate).InclusiveBetween(1, 5)
            .WithMessage("Rate must be between 1 and 5.");
    }
}

