# SDApp

## Local development configuration

Configuration needed for local development that is specific to your machine or is sensitive (API keys, passwords, connection strings with credentials etc.) should be stored using [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) rather than in `appsettings.Development.json`.
User secrets are stored outside of the repository so they cannot be accidentally committed, and they are only loaded when running in the `Development` environment.
`appsettings.Development.json` should only contain non-sensitive defaults that are shared by everyone working on the project.

To set a secret, run the following command from the `src/SDApp.Web` directory:

```shell
dotnet user-secrets set "<Key>" "<Value>"
```

Nested configuration keys are separated with a colon e.g. `ConnectionStrings:AppDb`.

To list the secrets you have set, run:

```shell
dotnet user-secrets list
```

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