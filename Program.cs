using Microsoft.EntityFrameworkCore;
using NewsAPIMobilePark.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<NewsContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddSingleton<NewsProcessor>();

var app = builder.Build();
app.Services.GetService<NewsProcessor>()?.ConfigureLanguages();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");
app.Run();
