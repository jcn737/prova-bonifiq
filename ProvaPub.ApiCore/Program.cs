using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Infrastructure.Data;
using ProvaPub.Infrastructure.Repository;
using ProvaPub.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IPaymentStrategy, PixPayment>();
builder.Services.AddScoped<IPaymentStrategy, CreditCardPayment>();
builder.Services.AddScoped<IPaymentStrategy, PaypalPayment>();
builder.Services.AddScoped<RandomService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddScoped<ProductService>();


builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
