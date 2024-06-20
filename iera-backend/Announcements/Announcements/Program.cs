using Announcements.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

partial class Program { }