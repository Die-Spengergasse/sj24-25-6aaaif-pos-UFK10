using System;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Manager : Employee
    {
#pragma warning disable CS8618
        protected Manager() { }
#pragma warning restore CS8618

        public Manager(int registrationNumber, string firstName, string lastName,
      Address? address, string carType, DateTime? lastUpdate = null, string? department = null)
      : base(registrationNumber, firstName, lastName, address)
        {
            CarType = carType;
            LastUpdate = lastUpdate ?? DateTime.UtcNow;
            Department = department;
        }


        [MaxLength(255)]
        public string CarType { get; set; }

        public DateTime? LastUpdate { get; set; }
        public string? Department { get; set; }
    }
}
