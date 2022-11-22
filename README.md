# Noda Time Test Drive
Testing driving the features of https://nodatime.org/

## Overview

- `NodaTimeTestDrive` uses NodaTime to:
  - implement a `ClockService` that exposes different properties/methods to allow the caller to make the correct use of the different types
  - implement a `WorldClockService` that generates the time of day in different timezones
- `NodaTimeTestDrive.Tests` contains unit tests for the above
- `NodaTimeTestDrive.WebApi` exposes parts of the services above to allow their use via REST endpoints and uses the NodaTime serialisation packages
- `NodaTimeTestDrive.Web` uses the web api to render SVG clocks, powered by the `WorldClockService`
- `NodaTimeTestDrive.Console` is juts a scratch project to play about with NodaTime

## Running the web app
- Open a prompt in the root of the repo
- Run the web api project
```
dotnet run --project ./NodaTimeTestDrive.WebApi/NodaTimeTestDrive.WebApi.csproj --launch-profile https
```
- (you can view the OpenApi page at https://localhost:7103/swagger/index.html)


- Open another prompt in the root of the repo
- Run the web project
```
dotnet run --project ./NodaTimeTestDrive.Web/NodaTimeTestDrive.Web.csproj --launch-profile https
```
- Open your browser to https://localhost:7049/