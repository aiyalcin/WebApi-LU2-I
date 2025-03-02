using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Ensure secrets are added to the configuration
builder.Configuration.AddUserSecrets<Program>();

// Retrieve the connection string securely
string connStr = builder.Configuration.GetConnectionString("ConnectionString1");

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;

        //REMOVE BEFORE FINAL 
        //options.Password.RequireDigit = false;
        //options.Password.RequireLowercase = false;
        //options.Password.RequireNonAlphanumeric = false;
        //options.Password.RequireUppercase = false;

    }).AddRoles<IdentityRole>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = connStr;
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();