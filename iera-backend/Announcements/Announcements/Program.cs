using Amazon.Util;
using Announcements.Data;
using Google.Cloud.Firestore;
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddSingleton<RabbitMQUserConsumer>();
builder.Services.AddSingleton<RabbitMQCommentConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Announcements - v1");
    }
    );
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Start RabbitMQ Consumer
var rabbitUserMQConsumer = app.Services.GetRequiredService<RabbitMQUserConsumer>();
var rabbitMQCommentConsumer = app.Services.GetRequiredService<RabbitMQCommentConsumer>();

// Graceful shutdown
app.Lifetime.ApplicationStopping.Register(() => 
{
    rabbitUserMQConsumer.Close();
});

app.Run();

partial class Program { }
