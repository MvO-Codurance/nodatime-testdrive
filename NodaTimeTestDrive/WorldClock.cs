using NodaTime;

namespace NodaTimeTestDrive;

public record struct WorldClock(DateTimeZone Timezone, Instant Instant, ZonedDateTime ZonedDateTime, LocalDateTime LocalDateTime);