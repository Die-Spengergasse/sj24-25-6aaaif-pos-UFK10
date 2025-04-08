using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Model;

namespace SPG_Fachtheorie.Aufgabe1.Services
{
    // Einfache Service-Klasse zum Handling von Zahlungen
    public class PaymentService
    {
        private readonly AppointmentContext _context;

        // Dependency Injection: Der DbContext wird über den Konstruktor eingebunden.
        public PaymentService(AppointmentContext context)
        {
            _context = context;
        }

        // Gibt alle Zahlungen zurück
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        // Gibt eine einzelne Zahlung anhand der ID zurück
        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                throw new KeyNotFoundException("Payment not found.");
            }
            return payment;
        }


        // Erzeugt eine neue Zahlung und speichert sie in der Datenbank
        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
