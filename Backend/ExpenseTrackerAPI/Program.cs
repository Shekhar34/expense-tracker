using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MySQL database connection
builder.Services.AddDbContext<ExpenseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21)))); // Adjust the MySQL version as per your installation

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense Tracker API", Version = "v1" });
});


// Add CORS policy to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allow all origins
                  .AllowAnyHeader()   // Allow all headers
                  .AllowAnyMethod();  // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
        });
});




// Add controllers (if needed)
builder.Services.AddControllers(); // If you are using controllers

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracker API v1");
    });
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers(); // Map controllers (if you are using them)

app.Run();
