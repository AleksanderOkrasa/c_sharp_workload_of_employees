using Microsoft.EntityFrameworkCore;
using WorkLoadAPI.Data;
using WorkLoadAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(connStr)); 

// Add services to the container.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.

app.MapGet("api/employees", async (AppDbContext db) =>
    await db.Employees.ToListAsync());
app.MapGet("api/employees/{id}", async (int id, AppDbContext db) =>
    await db.Employees.FindAsync(id)
        is Employee employee ? Results.Ok(employee) : Results.NotFound());


app.Run();

