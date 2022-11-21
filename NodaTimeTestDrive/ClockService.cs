using System.Globalization;
using NodaTime;
using NodaTime.Extensions;
using TimeZoneNames;

namespace NodaTimeTestDrive;

public class ClockService : IClockService
{
    private readonly IClock _clock;
    private readonly IDateTimeZoneProvider _timezoneProvider;

    public DateTimeZone TimeZone { get; }
    public Instant Now => _clock.GetCurrentInstant();
    public LocalDateTime LocalNow => Now.InZone(TimeZone).LocalDateTime;
    
    public ClockService(IClock clock, IDateTimeZoneProvider timezoneProvider)
    {
        _clock = clock;
        _timezoneProvider = timezoneProvider;
        
        // TODO populate the timezone from application or user settings 
        // for now, default to Europe/London
        TimeZone = DateTimeZoneProviders.Tzdb["Europe/London"];
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
    public IEnumerable<TimezoneForDisplay> GetTimezonesForDisplay()
    {
        return GetTimezonesForDisplay(CultureInfo.CurrentUICulture.Name);
    }
    
    public IEnumerable<TimezoneForDisplay> GetTimezonesForDisplay(string languageCode)
    {
        var list = TZNames.GetDisplayNames(languageCode: languageCode, useIanaZoneIds: true);
        return list
            .Select(selector: entry => new TimezoneForDisplay(Id: entry.Key, Name: entry.Value))
            .OrderBy(keySelector: x => x.Name);    
    }
    
    public Instant ToInstant(LocalDateTime local)
    {
        throw new NotImplementedException();
    }

    public LocalDateTime ToLocal(Instant instant)
    {
        throw new NotImplementedException();
    }
}