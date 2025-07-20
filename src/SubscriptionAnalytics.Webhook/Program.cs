var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// No Swagger or OpenAPI for now

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.Run();


public partial class Program { }
