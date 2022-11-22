namespace NodaTimeTestDrive.Tests;

public class WorldClockServiceShould
{
    [Theory]
    [InlineData("Europe/London")]
    [InlineData("Europe/Istanbul")]
    public void Resolve_The_Correct_Single_Timezone(string timezoneId)
    {
        var sut = GetWorldClockService();

        List<WorldClock> actualClocks = sut.GetClocks(timezoneId).ToList();

        actualClocks.Should().HaveCount(1);
        
        var worldClock = actualClocks[0]; 
        worldClock.Should().NotBeNull();
        worldClock.TimezoneId.Should().Be(timezoneId);
        worldClock.Timezone.Should().NotBeNull();
        worldClock.Timezone.Id.Should().Be(timezoneId);
    }
    
    [Theory]
    [InlineData("Europe/London", "Europe/Istanbul")]
    public void Resolve_The_Correct_Multiple_Timezones(params string[] timezoneIds)
    {
        var sut = GetWorldClockService();

        List<WorldClock> actualClocks = sut.GetClocks(timezoneIds).ToList();

        actualClocks.Should().HaveCount(timezoneIds.Length);

        var londonClock = actualClocks[0]; 
        londonClock.Should().NotBeNull();
        londonClock.TimezoneId.Should().Be(timezoneIds[0]);
        londonClock.Timezone.Should().NotBeNull();
        londonClock.Timezone.Id.Should().Be(timezoneIds[0]);
        
        var istanbulClock = actualClocks[1]; 
        istanbulClock.Should().NotBeNull();
        istanbulClock.TimezoneId.Should().Be(timezoneIds[1]);
        istanbulClock.Timezone.Should().NotBeNull();
        istanbulClock.Timezone.Id.Should().Be(timezoneIds[1]);
    }
    
    [Theory]
    [InlineData("Europe/London", "2022-11-20T10:30:00Z", "2022-11-20T10:30:00 Europe/London (+00)", "20/11/2022 10:30:00")]
    [InlineData("Europe/Istanbul", "2022-11-20T10:30:00Z", "2022-11-20T13:30:00 Europe/Istanbul (+03)", "20/11/2022 13:30:00")]
    public void Resolve_The_Correct_Times_For_A_Single_Timezone(
        string timezoneId,
        string instantDatetime,
        string zonedDateTime,
        string localDateTime)
    {
        var sut = GetWorldClockService();

        List<WorldClock> actualClocks = sut.GetClocks(timezoneId).ToList();

        actualClocks.Should().HaveCount(1);
        
        var worldClock = actualClocks[0]; 
        worldClock.Should().NotBeNull();
        worldClock.Instant.ToString().Should().Be(instantDatetime);
        worldClock.ZonedDateTime.ToString().Should().Be(zonedDateTime);
        worldClock.LocalDateTime.ToString().Should().Be(localDateTime);
    }
    
    private static WorldClockService GetWorldClockService()
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        return new WorldClockService(clock);
    }
}