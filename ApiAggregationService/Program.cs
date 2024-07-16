using ApiAggregationService.Controllers;
using ApiAggregationService.Models;
using ApiAggregationService.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<APIKeys>(Configuration.GetSection("ApiKeys"));

builder.Services.AddHttpClient<NewsService>();
builder.Services.AddHttpClient<TwitterService>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddTransient<AggregationController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
