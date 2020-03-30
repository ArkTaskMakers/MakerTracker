# MakerTracker

Built with ASP.NET Core 3.1, with a SQL Server Database

## Requires:
 Visual Studio 2019 (https://visualstudio.microsoft.com/vs/). The free community edition will do.
 or VSCode (mac or windows)
 A SQL Server Database
 
## Local Setup 
  1. Clone this repo 
  2. Update the 'DefaultConnection' in AppSettings.Json to point your sql database
  3. Build the project
  4. Update your database - From the Package Manager Console - run `dotnet ef database update --project MakerTracker` 
  5. Run it!
 
