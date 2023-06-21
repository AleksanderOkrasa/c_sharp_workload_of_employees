using Microsoft.EntityFrameworkCore;
using WorkLoad;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DbInter>(option => option.UseSqlite(connStr));   
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/api/employees", async (DbInter db) => await db.Employees.ToListAsync());

app.Run();

