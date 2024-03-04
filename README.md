# KYC360 Internship Task-2

## Connect Microsoft SQL Server via Connection String in appsettings.json

1. Open the `appsettings.json` file in your project.

2. Locate the section related to the database connection, usually named `"DefaultConnection"`. Update the `"ConnectionString"` value with your SQL Server connection details.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User=your_user;Password=your_password;"
      },
      // ... other settings
    }
    ```

3. Save the changes in `appsettings.json`.

## Initialize Database with Data

1. Open Visual Studio Package Manager Console.

2. Run the following commands to initialize the database with data:

    ```powershell
    Add-Migration
    Update-Database
    ```

## Seed Data using PowerShell

1. Open PowerShell in the root directory of your project.

2. Run the following command to seed the database with initial data:

    ```powershell
    dotnet run seeddata
    ```

Now, your project and database should be ready to go!

---

**Note:** Ensure you have the necessary dependencies installed and configured for your project. Adjust the connection string parameters according to your SQL Server setup.
