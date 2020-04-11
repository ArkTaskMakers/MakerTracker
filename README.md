# Maker Tracker

This app is intended to give the ability for a community to monitor the supply and demand of PPE (Personal Protection Equipment). 

"Makers"  are able self-report on their progress for sourcing and creating PPE related items. Hospitals/Clinics/etc are able to request PPE from their community. And finally these groups of people are able to see in real-time the logistical information of where items are going.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

1. Windows or *nix machine 
2. .NET Core 3.1 installed
3. VS Code to work on Angular (or similiar editor)
4. Visual Studio 2019 to work on the C# API Side (VS Code will do also)
What things you need to install the software and how to install them

### Installing

A step by step series of examples that tell you how to get a development env running

1. Clone this repo!
2. Setup up your local sql database
```
 // Run from the same folder as the .sln
 dotnet ef database update
```
3. Run the project!
```
 // Run from the same folder as the .sln
 dotnet run
```
4. Open the site 
```
  goto https://localhost:5001
```

From there you can login/register an account. It's already configured to hit Auth0 that we have setup for development purposes.

The Angular site is located in the `/ClientApp` folder

## Built With

* [ASP.NET Core 3.1](http://www.dropwizard.io/1.0.2/docs/) - Used for our API calls from Angular
* [Angular 9](https://maven.apache.org/) - Our frontend site!
* [Yarn](https://yarnpkg.com/) - Package manager for the Angular
* [Typewriter](https://marketplace.visualstudio.com/items?itemName=frhagn.Typewriter) - Used to sync c# models to typescript

## Contributing

We take PR's! 

## Authors

See also the list of [contributors](https://github.com/ArkTaskMakers/MakerTracker/graphs/contributors) who participated in this project.
