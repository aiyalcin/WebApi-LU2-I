using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using WebApi.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddTransient<ITilesRepo, TilesRepo>();
builder.Services.AddTransient<IEnvironmentRepo, EnvironmentRepo>();
builder.Services.AddTransient<IObject2DRepo, Object2DRepo>();

string connStr = builder.Configuration.GetValue<string>("ConnectionString1");

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;

    }).AddRoles<IdentityRole>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = connStr;
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(connStr);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}");
app.MapControllers();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();