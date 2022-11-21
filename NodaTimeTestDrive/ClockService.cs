using System.Globalization;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.TimeZones;
using TimeZoneNames;

namespace NodaTimeTestDrive;

public class ClockService : IClockService
{
    private readonly IClock _clock;
    private readonly IDateTimeZoneProvider _timezoneProvider;

    public DateTimeZone TimeZone { get; }
    public Instant Now => _clock.GetCurrentInstant();
    public LocalDateTime LocalNow => Now.InZone(TimeZone).LocalDateTime;

    public ClockService(IClock clock, IDateTimeZoneProvider timezoneProvider, DateTimeZone? timezone = null)
    {
        _clock = clock;
        _timezoneProvider = timezoneProvider;
        
        // use the supplied timezone or use default
        // TODO populate the default timezone from application or user settings (for now, use "Europe/London")
        TimeZone = timezone ?? DateTimeZoneProviders.Tzdb["Europe/London"];;
    }
    
    // This returns all NodaTime (IANA) timezones but the main useful property is the actual Id
    public IEnumerable<DateTimeZone> GetAllTimezones() => _timezoneProvider.GetAllZones();
    
    /* This returns timezones that are suitable for display (using the Name property), mapped back to the IANA Id.
     * This is required to have a better UX for end-users.
     * As NodaTime doesn't include support for generating this list (see https://stackoverflow.com/a/61496018)
     * we use the "TimeZoneNames" NuGet package instead (see https://stackoverflow.com/a/62048885).
     * However, also note that whilst the "TimeZoneNames" NuGet package suggests it is no longer needed for .NET 6.0+
     * (see https://github.com/mattjohnsonpint/TimeZoneNames) this still leads to inconsistency between Windows and
     * Linux timezone ids. So, we use "TimeZoneNames" to ensure consistent use of the IANA timezone ids.
     */
    public IEnumerable<TimezoneForDisplay> GetTimezonesForDisplay() => GetTimezonesForDisplay(CultureInfo.CurrentUICulture.Name);

    public IEnumerable<TimezoneForDisplay> GetTimezonesForDisplay(string languageCode)
    {
        var list = TZNames.GetDisplayNames(languageCode: languageCode, useIanaZoneIds: true);
        return list
            .Select(selector: entry => new TimezoneForDisplay(Id: entry.Key, Name: entry.Value))
            .OrderBy(keySelector: x => x.Name);    
    }
    
    public Instant? ToInstant(LocalDateTime? local) => local?.InZone(TimeZone, Resolvers.LenientResolver).ToInstant();

    public LocalDateTime? ToLocal(Instant? instant) => instant?.InZone(TimeZone).LocalDateTime;
}