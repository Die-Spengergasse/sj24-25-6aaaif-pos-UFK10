using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe3.Services;


namespace SPG_Fachtheorie.Aufgabe3.Services
{
    // Diese einfache Service-Klasse liefert grundlegende Methoden zur Mitarbeiterverwaltung.
    public class EmployeeServices
    {
        private readonly AppointmentContext _db;

        // Der Konstruktor erhält den DbContext über Dependency Injection.
        public EmployeeServices(AppointmentContext db)
        {
            _db = db;
        }

        // Methode zum Abrufen eines Mitarbeiters anhand seiner Registrierungsnummer.
        public async Task<Employee?> GetEmployeeByRegistrationNumberAsync(int registrationNumber)
        {
            return await _db.Employees.FirstOrDefaultAsync(e => e.RegistrationNumber == registrationNumber);
        }

        // Optionale Methode: Erzeugt einen neuen Mitarbeiter.
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }
    }
}
