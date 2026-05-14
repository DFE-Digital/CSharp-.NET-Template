# CSharp-.NET-Template
A template for C# .NET development in Schools Digital

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