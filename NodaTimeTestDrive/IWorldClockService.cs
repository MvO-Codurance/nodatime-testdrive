namespace NodaTimeTestDrive;

public interface IWorldClockService
{
    IEnumerable<WorldClock> GetClocks(params string[] timezoneIds);
}