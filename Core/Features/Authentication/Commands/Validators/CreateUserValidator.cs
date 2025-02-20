using Core.Features.Authentication.Commands.Models;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Core.Features.Authentication.Commands.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region constructors
        public CreateUserValidator(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            ApplyValidationRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .MaximumLength(100).WithMessage("The Max Length is 100");
            // Todo : validate phone number with country code 
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .EmailAddress().WithMessage("InValid Email");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .MinimumLength(6).WithMessage("must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("must contain at least one digit.")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("must contain at least one non-alphanumeric character.");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password And Confirm Password Not Equals.");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Email)
                .MustAsync(IsEmailValid)
                .WithMessage("Email already exists or is deactivated. Please contact support.");
        }

        // Valid if user doesn't exist or is not deleted
        private async Task<bool> IsEmailValid(string email, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user is not null) return false;

            if (user is not null && user.IsDeleted) return false;
            return true;
        }
        #endregion

    }
}
