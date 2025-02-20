﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Authentication.Commands.Models;
using FluentValidation;

namespace Core.Features.Authentication.Commands.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {

        public LoginValidator()
        {
            ApplyValidationRules();
        }
        #region Methods
        public void ApplyValidationRules()
        {
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

        }
        #endregion
    }
}
