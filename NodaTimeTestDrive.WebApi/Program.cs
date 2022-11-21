using NodaTime;
using NodaTimeTestDrive;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddSingleton<IClock>(_ => SystemClock.Instance)
    .AddScoped<IClockService, ClockService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/timezones/all", (IClockService clockService) => clockService.GetAllTimezones());

app.MapGet("/timezones/for-display", (IClockService clockService) => clockService.GetTimezonesForDisplay());

app.Run();