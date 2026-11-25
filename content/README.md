# SDApp

## Database migrations

Ensure you have the Entity Framework Core tools installed. You can install them using the following command from the root directory:

```shell
dotnet tool restore
```

### Add a migration

From the root directory, run the following command:

```shell
dotnet ef migrations add <MigrationName> -p src/SDApp.Web
```

### Update the database

From the root directory, run the following command:

```shell
dotnet ef database update -p src/SDApp.Web
```

## Background jobs

Background jobs are run using [Hangfire](https://www.hangfire.io/) with jobs stored in the application's Postgres database (in the `hangfire` schema, which is created automatically on startup).
The Hangfire server runs within the web application.

### Dashboard

In Development the Hangfire dashboard is available at `/hangfire`.
It is not enabled in any other environment; an authorization policy must be specified before enabling it - see the comment in `Program.cs`.

### Removing Hangfire

If the application does not need background jobs:
1. remove the `Hangfire.AspNetCore` and `Hangfire.PostgreSql` package references from `src/SDApp.Web/SDApp.Web.csproj`;
2. remove the `AddHangfire`, `AddHangfireServer` and `MapHangfireDashboard` calls from `src/SDApp.Web/Program.cs`;
3. drop the `hangfire` schema from any existing databases.
