namespace SPG_Fachtheorie.Aufgabe3.Dtos
{
    public record PaymentDto(
            int Id,
            string EmployeeFirstName,
            string EmployeeLastName,
            int CashDeskNumber,
            string PaymentType,
            decimal TotalAmount,
        DateTime PaymentDateTime
        );

    public record PaymentDetailDto(
            int Id,
            string EmployeeFirstName,
            string EmployeeLastName,
            int CashDeskNumber,
            string PaymentType,
            int EmployeeRegistrationNumber,
            List<PaymentItemDto> PaymentItems,
            DateTime PaymentDateTime
          


        );
    public record PaymentItemDto(
            string ArticleName,
            int Amount,
            decimal Price,
            int PaymentId,
            DateTime? LastUpdated
        );
}
