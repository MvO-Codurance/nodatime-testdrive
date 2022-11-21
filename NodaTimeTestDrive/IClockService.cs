using NodaTime;

namespace NodaTimeTestDrive;

public interface IClockService
{
    IEnumerable<DateTimeZone> AllTimezones { get; }
    
    IEnumerable<TimezoneForDisplay> TimezonesForDisplay { get; }

    DateTimeZone TimeZone { get; }

    Instant Now { get; }

    LocalDateTime LocalNow { get; }

    Instant ToInstant(LocalDateTime local);

    LocalDateTime ToLocal(Instant instant);
}