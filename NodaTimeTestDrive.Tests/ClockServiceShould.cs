namespace NodaTimeTestDrive.Tests;

public class ClockServiceShould
{
    [Fact]
    public void Get_A_List_Of_All_Timezones()
    {
        var sut = GetClockService();

        var timezones = sut.GetAllTimezones().ToList();

        timezones.Should().HaveCount(595);
        timezones[0].Should().Be("Africa/Abidjan");
        timezones[594].Should().Be("Zulu");
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

        var allTimezoneIds = sut.GetAllTimezones().ToList();
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
    
    [Theory]
    [InlineData("Europe/London", "20/11/2022 10:30:00")]
    [InlineData("Europe/Istanbul", "20/11/2022 13:30:00")]
    public void Returns_Correct_LocalNow_For_The_Given_Timezone(string timezoneId, string expectedLocalNow)
    {
        var sut = GetClockService(timezoneId);

        sut.LocalNow.ToString().Should().Be(expectedLocalNow);
    }
    
    [Fact]
    public void Returns_Null_Instant_For_Null_LocalDateTime()
    {
        var sut = GetClockService();

        sut.ToInstant(null).Should().BeNull();
    }
    
    [Theory]
    [InlineData("Europe/London", "2022-11-20T10:30:00Z")]
    [InlineData("Europe/Istanbul", "2022-11-20T07:30:00Z")]
    public void Returns_Correct_Instant_For_LocalDateTime_For_The_Given_Timezone(
        string timezoneId, 
        string expectedInstant)
    {
        var sut = GetClockService(timezoneId);
        var localDateTime = new LocalDateTime(2022, 11, 20, 10, 30);
        
        sut.ToInstant(localDateTime).ToString().Should().Be(expectedInstant);
    }
    
    [Fact]
    public void Returns_Null_LocalDateTime_For_Null_Instant()
    {
        var sut = GetClockService();

        sut.ToLocal(null).Should().BeNull();
    }
    
    [Theory]
    [InlineData("Europe/London", "20/11/2022 10:30:00")]
    [InlineData("Europe/Istanbul", "20/11/2022 13:30:00")]
    public void Returns_Correct_LocalDateTime_For_Instant_For_The_Given_Timezone(
        string timezoneId, 
        string expectedLocal)
    {
        var sut = GetClockService(timezoneId);
        var instant = Instant.FromUtc(2022, 11, 20, 10, 30);
        
        sut.ToLocal(instant).ToString().Should().Be(expectedLocal);
    }

    private static ClockService GetClockService(string? timezoneId = null)
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        DateTimeZone? timezone = null;

        if (!string.IsNullOrWhiteSpace(timezoneId))
        {
            timezone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezoneId);
        }
        
        var sut = new ClockService(clock, timezone);
        return sut;
    }
}