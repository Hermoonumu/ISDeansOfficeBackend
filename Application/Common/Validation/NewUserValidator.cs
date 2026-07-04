namespace DeanInfoSystem.Application.Validation;

using DeanInfoSystem.Application.DTO;
using FluentValidation;

public class NewUserValidator : AbstractValidator<NewUserDTO>
{
    public NewUserValidator()
    {
        RuleFor(x => x.BirthDate)
        .GreaterThan(DateTime.UtcNow - TimeSpan.FromDays(5110)).WithMessage("The person must be older than 14");

        RuleFor(x => x.FirstName)
        .MinimumLength(1).WithMessage("Name must be at least 1 character long")
        .MaximumLength(128).WithMessage("Name must be less than 128 characters")
        .Matches(@"^([А-ЩЬЮЯҐЄІЇа-щьюяґєії\s'\-]+|[A-Za-z\s'\-]+)$").WithMessage("Must be either ukr cyrillic or latin only.");

        RuleFor(x => x.LastName)
        .MinimumLength(1).WithMessage("Name must be at least 1 character long")
        .MaximumLength(128).WithMessage("Name must be less than 128 characters")
        .Matches(@"^([А-ЩЬЮЯҐЄІЇа-щьюяґєії\s'\-]+|[A-Za-z\s'\-]+)$").WithMessage("Must be either ukr cyrillic or latin only.");

        RuleFor(x => x.Password)
        .MinimumLength(8).WithMessage("The password has to be at least 8 characters long")
        .MaximumLength(255).WithMessage("The password has to be smaller than 255 characters")
        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
        .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
        .Matches("[0-9]").WithMessage("Password must contain at least one number.")
        .Matches(@"[!@#$%^&*()_+=\-[\]{}|;:',.<>?/~`""\\]").WithMessage("Password must contain at least one special character.");


        RuleFor(x => x.Username)
        .MinimumLength(6).WithMessage("Username has to be at least 6 character long")
        .MaximumLength(32).WithMessage("Username has to be shorter than 32 characters")
        .Matches("^[A-Za-z0-9]+$").WithMessage("Username has to consist of latin characters or numbers");
    }
}