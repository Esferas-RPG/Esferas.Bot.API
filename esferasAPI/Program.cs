using apiEsferas.Application.Sevices;
using apiEsferas.Domain.Interfaces;
using apiEsferas.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IGoogleApiService, GoogleApiService>();
builder.Services.AddScoped<GoogleApiAppService>();
builder.Services.AddHostedService<HeartbeatService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
