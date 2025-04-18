﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;       // Für Include() und FirstOrDefaultAsync()
using SPG_Fachtheorie.Aufgabe1.Dtos;
using SPG_Fachtheorie.Aufgabe1.Infrastructure; // dein DbContext (AppointmentContext)
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe3.Dtos;
using System.Linq;
using System.Net; // optional, wenn du HttpStatusCode nutzt


namespace SPG_Fachtheorie.Aufgabe3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AppointmentContext _db;

        public PaymentsController(AppointmentContext db)
        {
            _db = db;
        }


        [HttpGet]
        public ActionResult<IEnumerable<PaymentDto>> GetPayments([FromQuery] int? number, [FromQuery] DateTime? dateFrom)
        {
            var result = _db.Payments
                .Where(p => number == null || p.CashDesk.Number == number)
                .Where(p => dateFrom == null || p.PaymentDateTime >= dateFrom)
                .Select(p => new PaymentDto(
                    p.Id,
                    p.Employee.FirstName,
                    p.Employee.LastName,
                    p.CashDesk.Number,
                    p.PaymentType.ToString(),
                    p.PaymentItems.Sum(i => i.Price),
                    p.PaymentDateTime
                ))
                .ToList();

            return Ok(result);
        }

        // BEREITS VORHANDEN:
        [HttpGet("{id}")]
        public ActionResult<PaymentDetailDto> GetPayment(int id)
        {
            var data = _db.Payments
                .Where(p => p.Id == id)
                .Select(p =>
                    new PaymentDetailDto(
                        p.Id,
                        p.Employee.FirstName,
                        p.Employee.LastName,
                        p.CashDesk.Number,

                        p.PaymentType.ToString(),
                        p.Employee.RegistrationNumber,
                        p.PaymentItems.Select(i =>
                            new PaymentItemDto(
                               
                                i.ArticleName,
                                i.Amount,
                                i.Price,
                                 i.PaymentId,
                                i.LastUpdated
                            )


                        ).ToList(),
                        p.PaymentDateTime

                    )
                ).FirstOrDefault();

            if (data is null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] NewPaymentCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Prüfe PaymentDateTime: darf nicht mehr als 1 Minute in der Zukunft liegen
            if (command.PaymentDateTime > DateTime.Now.AddMinutes(1))
                return BadRequest("Payment date time cannot be more than 1 minute in the future.");

            // CashDesk suchen
            var cashDesk = await _db.CashDesks.FirstOrDefaultAsync(c => c.Number == command.CashDeskNumber);
            if (cashDesk == null)
                return BadRequest("Cash desk not found.");

            // Employee suchen
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.RegistrationNumber == command.EmployeeRegistrationNumber);
            if (employee == null)
                return BadRequest("Employee not found.");

            // PaymentType parsen
            if (!Enum.TryParse<PaymentType>(command.PaymentType, true, out var paymentType))
                return BadRequest("Payment type not recognized.");

            var payment = new Payment
            {
                CashDesk = cashDesk,
                PaymentDateTime = command.PaymentDateTime,
                PaymentType = paymentType,
                Employee = employee,
                PaymentItems = new List<PaymentItem>()
            };

            try
            {
                _db.Payments.Add(payment);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Payment: {ex.Message}");
            }
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, new { payment.Id });
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id, [FromQuery] bool deleteItems = false)
        {
            var payment = await _db.Payments
                .Include(p => p.PaymentItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            if (!deleteItems && payment.PaymentItems.Any())
            {
                return BadRequest("Payment has payment items.");
            }

            try
            {
                if (deleteItems)
                {
                    _db.PaymentItems.RemoveRange(payment.PaymentItems);
                }
                // dann Payment löschen
                _db.Payments.Remove(payment);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting Payment: {ex.Message}");
            }

            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePaymentConfirmed(int id, [FromBody] UpdateConfirmedCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payment = await _db.Payments.FindAsync(id);
            if (payment == null)
                return NotFound("Payment not found.");

            if (payment.Confirmed != null)
                return BadRequest("Payment already confirmed.");

            payment.Confirmed = command.Confirmed;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating payment: {ex.Message}");
            }

            return NoContent();
        }





    [HttpPut("paymentItems/{id}")]
        public async Task<IActionResult> UpdatePaymentItem(int id, [FromBody] UpdatePaymentItemCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != command.Id)
                return BadRequest("Invalid payment item ID.");

            var paymentItem = await _db.PaymentItems.FindAsync(id);
            if (paymentItem == null)
                return NotFound("Payment Item not found.");

            
            if (command.LastUpdated.HasValue && paymentItem.LastUpdated.HasValue &&
                command.LastUpdated.Value != paymentItem.LastUpdated.Value)
            {
                return BadRequest("Payment item has changed.");
            }

            // Überprüfe, ob das zugehörige Payment existiert.
            var payment = await _db.Payments.FindAsync(command.PaymentId);
            if (payment == null)
                return BadRequest("Invalid payment ID.");

            
            paymentItem.ArticleName = command.ArticleName;
            paymentItem.Amount = command.Amount;
            paymentItem.Price = command.Price;
            
            paymentItem.LastUpdated = DateTime.Now;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating payment item: {ex.Message}");
            }

            return NoContent();
        }



    } }

