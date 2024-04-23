
# Project Setup Instructions

## Database Configuration

First, you need to configure the database to run the project.

In the `Bluesoft.Bank.API` project, locate the `appsettings.json` file. Here, you should update the value of `ConnectionStrings.MainDB`. The current value should automatically create a database to run the code.

## Running Migrations

Once the database is configured, you need to run the project migrations to create the data structure in the database. There are several ways to do this. Below is the command to run migrations via the command line:

```bash
dotnet ef database update -p "Bluesoft.Bank.DataAccess.EF\Bluesoft.Bank.DataAccess.EF.csproj" -s "Bluesoft.Bank.API\Bluesoft.Bank.API.csproj"
```

This command should be executed in the root directory of the solution.

## Running the Project

Once the database is set up, you can run the project. The command to run it from the root directory is:

```bash
dotnet run --project "Bluesoft.Bank.API\Bluesoft.Bank.API.csproj" -lp https
```

This will run the API at https://localhost:7193/. If you navigate to https://localhost:7193/swagger/index.html, you can see the Swagger page with the API information.