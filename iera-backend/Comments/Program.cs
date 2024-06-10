using Comments.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var credentialsPath = builder.Configuration["GoogleCloud:CredentialsFilePath"];
if (string.IsNullOrEmpty(credentialsPath))
{
    credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
}

if (string.IsNullOrEmpty(credentialsPath))
{
    throw new InvalidOperationException("Google Cloud credentials file path is not set.");
}

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICommentRepository, CommentRepository>();

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

app.Run();
