var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.Run();


