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

// UTC London with pattern
DateTimeZone londonZone = DateTimeZoneProviders.Tzdb["Europe/London"];
ZonedClock londonClock = SystemClock.Instance.InZone(londonZone);
ZonedDateTime londonNow = londonClock.GetCurrentZonedDateTime();
ZonedDateTimePattern pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(pattern.Format(londonNow));

// UTC now in Hawaii
DateTimeZone hawaiiZone = DateTimeZoneProviders.Tzdb["US/Hawaii"];
ZonedDateTime hawaiiNow = instant.InZone(hawaiiZone);
Console.WriteLine(hawaiiNow.ToString());



