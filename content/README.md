# SDApp

## Client-side libraries

Client-side libraries (JavaScript, CSS etc.) should be managed using [LibMan](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/) rather than npm.
Libraries are declared in `src/SDApp.Web/libman.json` and are restored automatically as part of the build; restored files are not committed to source control.
By default, libraries are restored from [jsDelivr](https://www.jsdelivr.com/). Libraries should be installed into `wwwroot/lib/<library>`; this directory is ignored by git.

Ensure you have the LibMan CLI installed. You can install it using the following command from the root directory:

```shell
dotnet tool restore
```

### Add a library

From the `src/SDApp.Web` directory, run the following command:

```shell
dotnet libman install <library>@<version> --destination wwwroot/lib/<library>
```

Use `--files <file>` to restore only the files you need rather than the whole package.

### Update a library

Dependabot does not support LibMan, so libraries must be kept up to date manually.
From the `src/SDApp.Web` directory, run the following command:

```shell
dotnet libman update <library>
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