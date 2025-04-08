using System;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe3.Dtos
{
    public class PaymentDetailDtos
    {
        // Parameterloser Konstruktor (nützlich für z. B. Serialisierung)
        public PaymentDetailDtos()
        {
            PaymentItems = new List<PaymentItemDto>();
        }

        // Vollständiger Konstruktor, falls du direkt alle Werte zuweisen möchtest
        public PaymentDetailDtos(
            int id,
            string employeeFirstName,
            string employeeLastName,
            int cashDeskNumber,
            string paymentType,
            List<PaymentItemDto> paymentItems,
            DateTime? confirmed
        )
        {
            Id = id;
            EmployeeFirstName = employeeFirstName;
            EmployeeLastName = employeeLastName;
            CashDeskNumber = cashDeskNumber;
            PaymentType = paymentType;
            PaymentItems = paymentItems ?? new List<PaymentItemDto>();
            Confirmed = confirmed;
        }

        public int Id { get; set; }
        public string EmployeeFirstName { get; set; } = default!;
        public string EmployeeLastName { get; set; } = default!;
        public int CashDeskNumber { get; set; }
        public string PaymentType { get; set; } = default!;
        public DateTime PaymentDateTime { get; set; }


        // Liste der Items (Artikel), die zu diesem Payment gehören
        public List<PaymentItemDto> PaymentItems { get; set; }

        // Hier kannst du das Datum speichern, an dem das Payment bestätigt wurde (oder null, falls nicht bestätigt)
        public DateTime? Confirmed { get; set; }
    }
}
