using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe3.Dtos
{
    public class UpdatePaymentItemCommand : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public int Amount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PaymentId must be greater than 0.")]
        public int PaymentId { get; set; }

        public DateTime? LastUpdated { get; set; } = null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id <= 0)
            {
                yield return new ValidationResult("Invalid ID; must be greater than 0.", new[] { nameof(Id) });
            }
            if (string.IsNullOrWhiteSpace(ArticleName))
            {
                yield return new ValidationResult("Article name cannot be empty.", new[] { nameof(ArticleName) });
            }
            
        }
    }
}
