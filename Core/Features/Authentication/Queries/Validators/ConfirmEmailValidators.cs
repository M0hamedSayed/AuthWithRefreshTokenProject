using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Authentication.Queries.Models;
using FluentValidation;

namespace Core.Features.Authentication.Queries.Validators
{
    public class ConfirmEmailValidators : AbstractValidator<ConfirmEmailQuery>
    {
        public ConfirmEmailValidators() 
        {
            ApplyValidationsRules();
        }

        public void ApplyValidationsRules()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");

            RuleFor(x => x.token)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required");
        }
    }
}
