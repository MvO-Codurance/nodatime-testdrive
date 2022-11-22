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
        
        var londonClock = actualClocks[0]; 
        londonClock.Should().NotBeNull();
        londonClock.TimezoneId.Should().Be(timezoneId);
        londonClock.Timezone.Should().NotBeNull();
        londonClock.Timezone.Id.Should().Be(timezoneId);
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
    
    private static WorldClockService GetWorldClockService()
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        return new WorldClockService(clock);
    }
}