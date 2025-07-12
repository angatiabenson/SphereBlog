using Microsoft.EntityFrameworkCore;
using SphereBlog.Data;
using SphereBlog.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register DB context
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    }); // Enables Swagger UI at /swagger
}

app.UseHttpsRedirection();

// Add the response wrapper middleware
app.UseMiddleware<ResponseWrapperMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
