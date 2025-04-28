
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe1.Services;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;


namespace SPG_Fachtheorie.Aufgabe1.Test
{
    public class PaymentServiceTests
    {
        private AppointmentContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppointmentContext(options);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsAllPayments()
        {
            var ctx = GetEmptyDbContext();
            ctx.Payments.Add(new Payment());
            ctx.Payments.Add(new Payment());
            await ctx.SaveChangesAsync();

            var service = new PaymentService(ctx);
            var payments = await service.GetAllPaymentsAsync();

            Assert.Equal(2, payments.Count);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsCorrectPayment()
        {
            var ctx = GetEmptyDbContext();
            ctx.Payments.Add(new Payment { Id = 1 });
            await ctx.SaveChangesAsync();

            var service = new PaymentService(ctx);
            var payment = await service.GetPaymentByIdAsync(1);

            Assert.NotNull(payment);
            Assert.Equal(1, payment.Id);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ThrowsIfNotFound()
        {
            var ctx = GetEmptyDbContext();
            var service = new PaymentService(ctx);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetPaymentByIdAsync(999);
            });
        }

        [Fact]
        public async Task CreatePaymentAsync_SavesPayment()
        {
            var ctx = GetEmptyDbContext();
            var service = new PaymentService(ctx);

            var payment = new Payment();
            var result = await service.CreatePaymentAsync(payment);

            Assert.NotNull(result);
            Assert.Single(ctx.Payments);
        }
    }
}
