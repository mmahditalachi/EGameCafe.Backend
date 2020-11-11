# EGameCafe Backend

EgGane cafe Backend project 

## What is EGameCafe ? 

EGameCafe is a new kide of gaming social media for auto match making and manul match making base on user gaming exp . 

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

