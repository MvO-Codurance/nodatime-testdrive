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
        actualClocks[0].Should().NotBeNull();
        actualClocks[0].TimezoneId.Should().Be(timezoneId);
        actualClocks[0].Timezone.Should().NotBeNull();
        actualClocks[0].Timezone.Id.Should().Be(timezoneId);
    }
    
    [Theory]
    [InlineData("Europe/London", "Europe/Istanbul")]
    public void Resolve_The_Correct_Multiple_Timezones(params string[] timezoneIds)
    {
        var sut = GetWorldClockService();

        List<WorldClock> actualClocks = sut.GetClocks(timezoneIds).ToList();

        actualClocks.Should().HaveCount(timezoneIds.Length);
        actualClocks[0].Should().NotBeNull();
        actualClocks[0].TimezoneId.Should().Be(timezoneIds[0]);
        actualClocks[0].Timezone.Should().NotBeNull();
        actualClocks[0].Timezone.Id.Should().Be(timezoneIds[0]);
    }
    
    private static WorldClockService GetWorldClockService()
    {
        var clock = new FakeClock(Instant.FromUtc(2022, 11, 20, 10, 30));
        return new WorldClockService(clock);
    }
}