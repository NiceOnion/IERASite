using Comments.Data;
using Comments.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<RabbitMQConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Announcements - v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Start RabbitMQ Consumer
var rabbitMQConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();

// Graceful shutdown
app.Lifetime.ApplicationStopping.Register(() =>
{
    rabbitMQConsumer.Close();
});

app.Run();

partial class Program { }
