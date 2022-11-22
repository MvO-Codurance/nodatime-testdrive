using NodaTime;

namespace NodaTimeTestDrive;

public record struct WorldClock(string TimezoneId, DateTimeZone Timezone, Instant Instant, ZonedDateTime ZonedDateTime, LocalDateTime LocalDateTime);