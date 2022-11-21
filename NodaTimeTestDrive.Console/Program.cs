using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using NodaTimeTestDrive;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddSingleton<IClock>(_ => SystemClock.Instance)
        .AddSingleton<IDateTimeZoneProvider>(_ => DateTimeZoneProviders.Tzdb)
        // IClockService is scoped as the Timezone could change on a per-user basis
        .AddScoped<IClockService, ClockService>())
    .Build();

var clockService = host.Services.GetRequiredService<IClockService>();
foreach (var tz in clockService.GetTimezonesForDisplay())
{
    Console.WriteLine($"{tz.Id} - {tz.Name}");
}

/*
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
 
// UTC now
Instant instant = SystemClock.Instance.GetCurrentInstant();
ZonedDateTime now = instant.InUtc();
Console.WriteLine(now.ToString());

// list timezones
IEnumerable<DateTimeZone> timezones = DateTimeZoneProviders.Tzdb.GetAllZones();
foreach (DateTimeZone timezone in timezones)
{
    Console.WriteLine($"{timezone.Id} {timezone.GetUtcOffset(instant)}");
}

// UTC now in London with pattern
DateTimeZone londonZone = DateTimeZoneProviders.Tzdb["Europe/London"];
ZonedClock londonClock = SystemClock.Instance.InZone(londonZone);
ZonedDateTime londonNow = londonClock.GetCurrentZonedDateTime();
ZonedDateTimePattern pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(pattern.Format(londonNow));

// UTC now in Hawaii
DateTimeZone hawaiiZone = DateTimeZoneProviders.Tzdb["US/Hawaii"];
ZonedDateTime hawaiiNow = instant.InZone(hawaiiZone);
Console.WriteLine(hawaiiNow.ToString());

// system timezones
foreach (TimeZoneInfo timezone in TimeZoneInfo.GetSystemTimeZones())
{
    Console.WriteLine($"{timezone.Id}/{timezone.StandardName}/{timezone.DisplayName}/{timezone.HasIanaId}");
}
*/