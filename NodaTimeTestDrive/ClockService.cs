using System.Globalization;
using NodaTime;
using NodaTime.Extensions;
using TimeZoneNames;

namespace NodaTimeTestDrive;

public class ClockService : IClockService
{
    private readonly IClock _clock;
    private readonly IDateTimeZoneProvider _timezoneProvider;
    
    public DateTimeZone TimeZone => throw new NotImplementedException();
    public Instant Now => throw new NotImplementedException();
    public LocalDateTime LocalNow => throw new NotImplementedException();
    
    // This returns all NodaTime (IANA) timezones but the main useful property is the actual Id
    public IEnumerable<DateTimeZone> AllTimezones => _timezoneProvider.GetAllZones();
    
    // This returns timezones that are suitable for display (using the Name property), mapped back to the IANA Id. 
    public IEnumerable<TimezoneForDisplay> TimezonesForDisplay {
        get
        {
            var list = TZNames.GetDisplayNames(CultureInfo.CurrentUICulture.Name, useIanaZoneIds:true);
            return list
                .Select(entry => new TimezoneForDisplay(entry.Key, entry.Value))
                .OrderBy(x => x.Name);    
        }
    }

    public ClockService(IClock clock, IDateTimeZoneProvider timezoneProvider)
    {
        _clock = clock;
        _timezoneProvider = timezoneProvider;
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