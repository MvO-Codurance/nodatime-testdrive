namespace NodaTimeTestDrive.Tests;

public class ClockServiceShould
{
    [Fact]
    public void Get_A_List_Of_All_Timezones()
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        var timezoneProvider = DateTimeZoneProviders.Tzdb;
        var sut = new ClockService(clock, timezoneProvider);

        var timezones = sut.GetAllTimezones().ToList();

        timezones.Should().HaveCount(595);
        timezones[0].Id.Should().Be("Africa/Abidjan");
        timezones[594].Id.Should().Be("Zulu");
    }
    
    [Fact]
    public void Get_A_List_Of_Timezones_For_Display()
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        var timezoneProvider = DateTimeZoneProviders.Tzdb;
        var sut = new ClockService(clock, timezoneProvider);

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
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        var timezoneProvider = DateTimeZoneProviders.Tzdb;
        var sut = new ClockService(clock, timezoneProvider);

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
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        var timezoneProvider = DateTimeZoneProviders.Tzdb;
        var sut = new ClockService(clock, timezoneProvider);

        var allTimezoneIds = sut.GetAllTimezones().Select(x => x.Id).ToList();
        var timezoneIdsForDisplay = sut.GetTimezonesForDisplay().Select(x => x.Id).ToList();

        timezoneIdsForDisplay.Except(allTimezoneIds).Should().HaveCount(0);
    }
}