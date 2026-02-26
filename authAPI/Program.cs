using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Configure in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryUserAuthApp")); //to store user data in memory for testing purposes. This is useful for development and testing scenarios where you don't want to set up a full database server. The UseInMemoryDatabase method is called on the DbContextOptionsBuilder to specify that an in-memory database should be used, and the name "InMemoryUserAuthApp" is given to the database.

// Add Identity, To add Identity services to the application, you can use the AddDefaultIdentity method, which sets up the default Identity system with the specified user type (in this case, IdentityUser). The AddEntityFrameworkStores method is then called to specify that the Identity system should use Entity Framework Core for data storage, and it takes the ApplicationDbContext as a parameter to indicate which DbContext should be used for storing Identity data.
builder.Services.AddDefaultIdentity<IdentityUser>() //IdentityUser It represents one user record in the database.
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();//redirects HTTP to HTTPS for security purposes
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
