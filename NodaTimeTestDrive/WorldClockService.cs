using NodaTime;
using NodaTime.Extensions;

namespace NodaTimeTestDrive;

public class WorldClockService
{
    private readonly IClock _clock;

    public WorldClockService(IClock clock)
    {
        _clock = clock;
    }
    
    public IEnumerable<WorldClock> GetClocks(params string[] timezoneIds)
    {
        List<WorldClock> worldClocks = new(timezoneIds.Length);

        foreach (string timezoneId in timezoneIds)
        {
            var timezone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezoneId);
            if (timezone == null)
            {
                continue;
            }
             
            var zonedClock = _clock.InZone(timezone);

            worldClocks.Add(
                new WorldClock(
                    timezoneId,
                    timezone,
                    _clock.GetCurrentInstant(),
                    zonedClock.GetCurrentZonedDateTime(),
                    zonedClock.GetCurrentLocalDateTime())
            );
        }

        return worldClocks;
    }
}