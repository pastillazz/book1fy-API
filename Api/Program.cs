using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.Run();


