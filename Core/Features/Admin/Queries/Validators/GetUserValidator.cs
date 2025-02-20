using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Admin.Queries.Models;
using FluentValidation;

namespace Core.Features.Admin.Queries.Validators
{
    public class GetUserValidator:AbstractValidator<GetUserQuery>
    {

        public GetUserValidator()
        {
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
        }
    }
}
