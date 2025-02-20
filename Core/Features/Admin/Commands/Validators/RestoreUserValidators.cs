using Core.Features.Admin.Commands.Models;
using FluentValidation;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Admin.Commands.Validators
{
    internal class RestoreUserValidators:AbstractValidator<RestoreUserQuery>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RestoreUserValidators( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ApplyValidationsRules();
            //ApplyCustomValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Must Not Empty")
                .NotNull().WithMessage("Is Required")
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Id)
                .MustAsync(IsUserIdValid)
                .WithMessage("User Not Found");
        }

        // Valid if user doesn't exist or is not deleted
        private async Task<bool> IsUserIdValid(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepoistory.GetTableNoTracking().AnyAsync(user => user.Id == id);
            if (!user) return false;
            return true;
        }
    }
}
