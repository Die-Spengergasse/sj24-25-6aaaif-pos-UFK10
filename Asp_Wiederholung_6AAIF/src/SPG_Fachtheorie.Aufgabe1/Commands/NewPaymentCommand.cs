using System;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Dtos
{
    public record NewPaymentCommand(
        [Required] int CashDeskNumber,
        [Required] DateTime PaymentDateTime,
        [Required] string PaymentType,
        [Required] int EmployeeRegistrationNumber
    );
}
