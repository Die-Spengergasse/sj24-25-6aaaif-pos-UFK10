using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Services;
using SPG_Fachtheorie.Aufgabe3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EmployeeServices>();

// Suche im Programmcode nach allen Klassen mit [ApiController]
builder.Services.AddControllers();

builder.Services.AddScoped<PaymentService>();

// SERVICE PROVIDER
builder.Services.AddDbContext<AppointmentContext>(opt =>
{
    opt.UseSqlite("DataSource=cash.db");
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    using (var db = scope.ServiceProvider.GetRequiredService<AppointmentContext>())
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Seed();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
