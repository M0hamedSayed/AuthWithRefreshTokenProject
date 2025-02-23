﻿using FluentValidation;
using MediatR;

namespace Core.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Fields
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        #endregion

        #region Constructors
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        #endregion

        #region Methods
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(res => res.Errors).Where(error => error is not null).ToList();

                if (failures.Count != 0)
                {
                    var message = failures.Select(error =>  $"{error.PropertyName} : {error.ErrorMessage}").FirstOrDefault();

                    throw new System.ComponentModel.DataAnnotations.ValidationException(message);
                }
            }
            return await next();  
        }
        #endregion

    }
}
