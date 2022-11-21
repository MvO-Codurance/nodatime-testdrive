namespace NodaTimeTestDrive.Tests;

public class ClockServiceShould
{
    [Fact]
    public void Get_A_List_Of_All_Timezones()
    {
        var sut = GetClockService();

        var timezones = sut.GetAllTimezones().ToList();

        timezones.Should().HaveCount(595);
        timezones[0].Id.Should().Be("Africa/Abidjan");
        timezones[594].Id.Should().Be("Zulu");
    }

    [Fact]
    public void Get_A_List_Of_Timezones_For_Display()
    {
        var sut = GetClockService();

        var timezones = sut.GetTimezonesForDisplay().ToList();

        timezones.Should().HaveCount(139);
        timezones[0].Id.Should().Be("Atlantic/Azores");
        timezones[0].Name.Should().Be("(UTC-01:00) Azores");
        timezones[138].Id.Should().Be("Pacific/Kiritimati");
        timezones[138].Name.Should().Be("(UTC+14:00) Kiritimati Island");
    }

    [Fact]
    public void Get_A_List_Of_Timezones_For_Display_In_French()
    {
        var sut = GetClockService();

        var timezones = sut.GetTimezonesForDisplay("fr").ToList();

        timezones.Should().HaveCount(139);
        timezones[0].Id.Should().Be("Atlantic/Cape_Verde");
        timezones[0].Name.Should().Be("(UTC-01:00) Îles de Cabo Verde");
        timezones[138].Id.Should().Be("Pacific/Kiritimati");
        timezones[138].Name.Should().Be("(UTC+14:00) Kiritimati, Île");
    }

    [Fact]
    public void Ensure_Ids_From_TimezonesForDisplay_Are_A_Subset_Of_AllTimezones()
    {
        var sut = GetClockService();

        var allTimezoneIds = sut.GetAllTimezones().Select(x => x.Id).ToList();
        var timezoneIdsForDisplay = sut.GetTimezonesForDisplay().Select(x => x.Id).ToList();

        timezoneIdsForDisplay.Except(allTimezoneIds).Should().HaveCount(0);
    }
    
    [Fact]
    public void Use_Europe_London_As_The_Default_Timezone()
    {
        var sut = GetClockService();

        sut.TimeZone.Id.Should().Be("Europe/London");
    }
    
    [Fact]
    public void Use_The_Specified_Timezone_As_The_Timezone()
    {
        var sut = GetClockService("Europe/Istanbul");

        sut.TimeZone.Id.Should().Be("Europe/Istanbul");
    }
    
    [Fact]
    public void Returns_Correct_Now_Instant()
    {
        var sut = GetClockService();

        sut.Now.ToString().Should().Be("2022-11-20T10:30:00Z");
    }
    
    [Fact]
    public void Returns_Correct_LocalNow_For_Europe_London_Timezone()
    {
        var sut = GetClockService();

        sut.LocalNow.ToString().Should().Be("20/11/2022 10:30:00");
    }
    
    [Fact]
    public void Returns_Null_Instant_For_Null_LocalDateTime()
    {
        var sut = GetClockService();

        sut.ToInstant(null).Should().BeNull();
    }
    
    [Fact]
    public void Returns_Correct_Instant_For_LocalDateTime_For_Europe_London_Timezone()
    {
        var sut = GetClockService();
        var localDateTime = new LocalDateTime(2022, 11, 20, 10, 30);
        
        sut.ToInstant(localDateTime).ToString().Should().Be("2022-11-20T10:30:00Z");
    }
    
    [Fact]
    public void Returns_Null_LocalDateTime_For_Null_Instant()
    {
        var sut = GetClockService();

        sut.ToLocal(null).Should().BeNull();
    }
    
    [Fact]
    public void Returns_Correct_LocalDateTime_For_Instant_For_Europe_London_Timezone()
    {
        var sut = GetClockService();
        var instant = Instant.FromUtc(2022, 11, 20, 10, 30);
        
        sut.ToLocal(instant).ToString().Should().Be("20/11/2022 10:30:00");
    }

    private static ClockService GetClockService(string? timezoneId = null)
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        var timezoneProvider = DateTimeZoneProviders.Tzdb;
        DateTimeZone? timezone = null;

        if (!string.IsNullOrWhiteSpace(timezoneId))
        {
            timezone = timezoneProvider.GetZoneOrNull(timezoneId);
        }
        
        var sut = new ClockService(clock, timezoneProvider, timezone);
        return sut;
    }
}