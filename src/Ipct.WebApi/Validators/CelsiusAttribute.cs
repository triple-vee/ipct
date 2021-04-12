using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ipct.WebApi.Validators
{
    /// <summary>
    /// Validates that the string representation of a phone number is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CelsiusAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the string representation of a phone number.
        /// </summary>
        /// <param name="value">The string representation of the phone number.</param>
        /// <param name="validationContext"><see cref="ValidationContext"/>.</param>
        /// <returns><see cref="ValidationResult"/>.</returns>
        /// <remarks>
        /// Does not modify the input string.
        /// </remarks>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var logger = validationContext.GetService<ILogger<CelsiusAttribute>>();

            // Do not validate that it is populated here. Use the Required Attribute if non-null is required.
            if (value == null)
            {
                return ValidationResult.Success;
            }

            int celsius = (int)value;

            if (celsius > -273)
            {
                return ValidationResult.Success;
            }

            var message = $"Can't be below absolute zero: {celsius}";
            logger.LogError(message);
            return new ValidationResult(message, new[] { nameof(celsius) });
        }
    }
}