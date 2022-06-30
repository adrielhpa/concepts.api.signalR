using Concepts.SignalR.Hubs;
using Concepts.SQS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(x => x.AddDefaultPolicy(x => x.SetIsOriginAllowed(x => true).AllowAnyHeader().AllowCredentials()));
builder.Services.AddSingleton<MessageHub>();
builder.Services.AddHostedService<SQSService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MessageHub>("/messagesHub");

app.Run();
