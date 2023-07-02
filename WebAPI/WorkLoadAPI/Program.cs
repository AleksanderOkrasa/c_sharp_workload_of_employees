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
#region EmployeeOps
//get employees
app.MapGet("api/employees", async (AppDbContext db) =>
    await db.Employees.ToListAsync());
//get employee by id
app.MapGet("api/employees/{id}", async (int id, AppDbContext db) =>
    await db.Employees.FindAsync(id)
        is Employee employee ? Results.Ok(employee) : Results.NotFound());
//add employee
app.MapPost("api/employees", async (Employee employee, AppDbContext db) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();

    return Results.Created($"/employees/{employee.Id}", employee);
});
//update employee
app.MapPut("/api/employees{id}", async (int id, Employee inputEmployee, AppDbContext db) =>
{
    var employee = await db.Employees.FindAsync(id);

    if (employee == null) return Results.NotFound();

    employee.FirstName = inputEmployee.FirstName;
    employee.LastName = inputEmployee.LastName;

    await db.SaveChangesAsync();

    return Results.NoContent();
});
//delete employee
app.MapDelete("/api/employees/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Employees.FindAsync(id) is Employee employee)
    {
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.Ok(employee);
    }

    return Results.NotFound();
});
#endregion EmployeeOps
#region DutiesOps
//get all duties
app.MapGet("/api/duties", async (AppDbContext db) =>
    await db.Duties.ToListAsync());
//get duty by id
app.MapGet("/api/duties/{id}", async (int id, AppDbContext db) => 
    await db.Duties.FindAsync(id)
        is Duty duty ? Results.Ok(duty) : Results.NotFound());
//add duty
app.MapPost("/api/duties", async (Duty duty, AppDbContext db) =>
{
    db.Duties.Add(duty);
    await db.SaveChangesAsync();

    return Results.Created($"/duties/{duty.Id}", duty);
});
//update duty
app.MapPut("/api/duties/{id}", async (int id, Duty inputDuty, AppDbContext db) =>
{
    var duty = await db.Duties.FindAsync(id);

    if (duty == null) return Results.NotFound();

    duty.DutyDescription = inputDuty.DutyDescription;
    duty.Time = inputDuty.Time;
    duty.Priority = inputDuty.Priority;
    duty.EmployeeId = inputDuty.EmployeeId;

    await db.SaveChangesAsync();

    return Results.NoContent();
});
//delete duty
app.MapDelete("/api/duties/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Duties.FindAsync(id) is Duty duty)
    {
        db.Duties.Remove(duty);
        await db.SaveChangesAsync();
        return Results.Ok(duty);
    }

    return Results.NotFound();
});
#endregion DutiesOps

app.Run();

