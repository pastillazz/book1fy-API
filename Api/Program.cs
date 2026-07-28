using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.Run();


