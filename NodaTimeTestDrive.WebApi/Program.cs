using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using NodaTimeTestDrive;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));
builder.Services
    .AddSingleton<IClock>(_ => SystemClock.Instance)
    .AddScoped<IClockService, ClockService>()
    .AddScoped<IWorldClockService, WorldClockService>();

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

app.MapGet("/worldclocks", (IWorldClockService worldClockService) => worldClockService.GetClocks(
    "America/Los_Angeles",
    "America/New_York",
    "Europe/London",
    "Europe/Paris",
    "Europe/Rome",
    "Asia/Tokyo"));

app.UseCors("AllowAllOrigins");

app.Run();