using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ipct.WebApi.Validators;

namespace Ipct.WebApi
{
    public class WeatherForecast /*: IValidatableObject */
    {
        public DateTime Date { get; set; }

        [Celsius]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Summary.Contains("5"))
            {
                yield return new ValidationResult(
                    "Summary contains the numeral 5",
                    new[] { nameof(Summary) });
            }

            if (TemperatureC < -100)
            {
                yield return new ValidationResult(
                    "Are you in space?",
                    new[] { nameof(TemperatureC) });
            }
        }
    }
}
