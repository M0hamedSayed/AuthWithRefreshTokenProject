
using Core.Features.Authentication.Queries.Models;
using FluentValidation;

namespace Core.Features.Authentication.Queries.Validators
{
    public class RecendConfirmEmailValidator : AbstractValidator<ResendConfirmEmailQuery>
    {
        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .EmailAddress().WithMessage("InValid Email");

        }
        #endregion
    }
}
