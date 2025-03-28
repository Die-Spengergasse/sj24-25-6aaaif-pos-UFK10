using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe3.Dtos
{
    public class UpdateConfirmedCommand : IValidatableObject
    {
        [Required]
        public DateTime Confirmed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Überprüfe, dass der bestätigte Timestamp nicht mehr als 1 Minute in der Zukunft liegt.
            if (Confirmed > DateTime.Now.AddMinutes(1))
            {
                yield return new ValidationResult(
                    "Confirmed date cannot be more than 1 minute in the future.",
                    new[] { nameof(Confirmed) });
            }
        }
    }
}
