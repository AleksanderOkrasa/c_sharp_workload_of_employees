using Microsoft.EntityFrameworkCore;
using WorkLoad;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DbInter>(option => option.UseSqlite(connStr));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

//Read all employees
app.MapGet("/employees", EmployeeOps.getAllEmployees);
//Insert employee
app.MapPost("/employess", EmployeeOps.insertEmployee);
//Read all tasks
app.MapGet("/tasks", TaskOps.getAllTasks);
//Insert task
app.MapPost("/tasks", TaskOps.insertTask);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

