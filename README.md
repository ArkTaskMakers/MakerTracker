# MakerTracker

Built with ASP.NET Core 3.1, with a SQL Server Database

## Requires:
 Visual Studio 2019 (https://visualstudio.microsoft.com/vs/). The free community edition will do.
 or VSCode (mac or windows)
 A SQL Server Database
 
## Local Setup 
  1. Clone this repo 
  2. Update the 'DefaultConnection' in AppSettings.Json to point your sql database.  See below for concerns regarding this string.
  3. Build the project
  4. Install dotnet entity framework core tools if not previously installed - From the Package Manager Console - run `dotnet tool install --global dotnet-ef` 
  5. Update your database - From the Package Manager Console - run `dotnet ef database update --project MakerTracker` 
  6. Run it!
 

## Database Connection String
Visual Studio offers a lite-version dev instance of MSSQL's engine as an option inside of Visual Studio's Installer.  This is also installed by default via the .Net Core package option inside of the Installer. You may also use a local full-installation of the engine.  Your connection string will differ depending on the option you choose.  For the lite-version, you'll use the following for server name:
 > Server=(localdb)\\mssqllocaldb;
 
 This leads to a full connection string of:
  > "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MakerTracker;Trusted_Connection=True;MultipleActiveResultSets=true"
 
 If you're instead using a full MSSQL instance, the server string needs to be slightly modified to:
  > Server=(local);
  
  Which leads to a full connection string of:
  > "DefaultConnection": "Server=(local);Database=MakerTracker;Trusted_Connection=True;MultipleActiveResultSets=true"
  
