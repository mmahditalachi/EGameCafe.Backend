# EGameCafe Backend

EGameCafe Backend project 

## What is EGameCafe ? 

EGameCafe is a new kind of gaming social media for auto match making and manual match making based on user gaming exp . 

### Features : 

Authentication and Authorization base on JWT 
Online Chat 
Gaming dashboard 

### Architecture : 

This project achitecture is base on Clean Architecture by [Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture "package page link") and robert c martin clean architecture .

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update WebUI/appsettings.json as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the DefaultConnection connection string within appsettings.json points to a valid SQL Server instance.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Getting Started

1. Install the latest [.NET Core SDK](https://dotnet.microsoft.com/download)
2. Navigate to EGameCafe.Server and run dotnet run to launch the back end (ASP.NET Core Web API) 

You can test authentican and other feature with sample users that creats in start up 

```json
    {
        "Id": "fc7b1548-cb36-43f1-8c96-4c5843629a68",
        "UserName" : "User_Test_1",
        "PhoneNumber" : "09354891892",
        "password": "password"
    },
    {
        "Id": "cc011c60-fc7f-4dd2-906e-f89b23796831"
        "UserName" : "User_Test_2",
        "PhoneNumber" : "0933333333",
        "password": "password"
    }
```

You can seed more data to database in ApplicationDbContextSeed.cs in EGameCafe.Infrastructure.Persistence namespace 
