using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe1.Services;
using SPG_Fachtheorie.Aufgabe1.Commands;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;

namespace SPG_Fachtheorie.Aufgabe1.Test
{
    public class EmployeeServiceTests
    {
        private AppointmentContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppointmentContext(options);
        }

        [Fact]
        public void AddManager_AddsManagerSuccessfully()
        {
            var ctx = GetEmptyDbContext();
            var service = new EmployeeService(ctx);

            var cmd = new NewManagerCmd(123, "Max", "Mustermann", new AddressCmd("Street", "12345", "City"), "SUV");
            var manager = service.AddManager(cmd);

            Assert.NotNull(manager);
            Assert.Equal("Max", manager.FirstName);
            Assert.Single(ctx.Managers);
        }

        [Fact]
        public void UpdateManager_UpdatesSuccessfully()
        {
            var ctx = GetEmptyDbContext();
            var manager = new Manager(123, "OldFirst", "OldLast", new Address("Street", "12345", "City"), "OldCar");
            ctx.Managers.Add(manager);
            ctx.SaveChanges();

            var service = new EmployeeService(ctx);
            var cmd = new UpdateManagerCmd(123, "NewFirst", "NewLast", new AddressCmd("NewStreet", "54321", "NewCity"), "NewCar", manager.LastUpdate);

            service.UpdateManager(cmd);

            var updatedManager = ctx.Managers.First();
            Assert.Equal("NewFirst", updatedManager.FirstName);
            Assert.Equal("NewLast", updatedManager.LastName);
        }

        [Fact]
        public void UpdateManager_ThrowsIfManagerNotFound()
        {
            var ctx = GetEmptyDbContext();
            var service = new EmployeeService(ctx);

            var cmd = new UpdateManagerCmd(999, "First", "Last", null, "Car", null);

            var ex = Assert.Throws<EmployeeServiceException>(() => service.UpdateManager(cmd));
            Assert.True(ex.NotFoundException);
        }

        [Fact]
        public void UpdateAddress_UpdatesAddressSuccessfully()
        {
            var ctx = GetEmptyDbContext();
            var manager = new Manager(123, "First", "Last", new Address("OldStreet", "12345", "City"), "Car");
            ctx.Managers.Add(manager);
            ctx.SaveChanges();

            var service = new EmployeeService(ctx);
            var cmd = new AddressCmd("NewStreet", "54321", "NewCity");

            service.UpdateAddress(123, cmd);

            var updatedManager = ctx.Managers.First();
            Assert.Equal("NewStreet", updatedManager.Address!.Street);

        }

        [Fact]
        public void UpdateAddress_ThrowsIfManagerNotFound()
        {
            var ctx = GetEmptyDbContext();
            var service = new EmployeeService(ctx);
            var cmd = new AddressCmd("Street", "12345", "City");

            var ex = Assert.Throws<EmployeeServiceException>(() => service.UpdateAddress(999, cmd));
            Assert.True(ex.NotFoundException);
        }

        [Fact]
        public void DeleteEmployee_DeletesSuccessfully()
        {
            var ctx = GetEmptyDbContext();
            var employee = new Manager(123, "First", "Last", new Address("Street", "12345", "City"), "Car");
            ctx.Employees.Add(employee);
            ctx.SaveChanges();

            var service = new EmployeeService(ctx);
            service.DeleteEmployee(123);

            Assert.Empty(ctx.Employees);
        }
    }
}
