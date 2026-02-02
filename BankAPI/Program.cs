using BankAPI.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BankAPI.Middleware;
using BankAPI.Repositories.Interfaces;
using BankAPI.Repositories;
using BankAPI.Services;
using BankAPI.Services.Interfaces;
using BankAPI.Validators.AccountValidators;
using BankAPI.Validators.ClientValidators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source = app.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClientRepository, EfClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, EfAccountRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientsUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AccountCreateValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<AccountUpdateValidators>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();