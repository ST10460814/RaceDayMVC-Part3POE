# RaceDayMVC- RaceDay Event Management Platform
## MVC Design Pattern
RaceDayMVC follows the Model-View-Controller architectural pattern. Models represent application data, Views provide the user interface, and Controllers process requests and coordinate interactions with the RESTful API.
This separation improves code organisation, maintainability, and the ability to extend the application.

## HTTP API Communication

The MVC application communicates with the TestingPoe RESTful API using HTTP requests. The EventController uses IHttpClientFactory to create a configured HTTP client.

GET requests retrieve event information, POST requests submit new information, PUT requests update existing resources, and DELETE requests request resource removal.

The MVC application checks API response status codes before displaying the results.

## Dependency Injection

RaceDay uses ASP.NET Core dependency injection to provide services to application controllers.

For example, IHttpClientFactory is injected into the MVC EventController, while ApplicationDbContext is injected into API controllers.

Dependency injection reduces tight coupling and makes services easier to configure, maintain, and test.

RaceDayMVC follows the Model-View-Controller architectural pattern. Models represent application data, Views provide the user interface, and Controllers process requests and coordinate interactions with the RESTful API.

This separation improves code organisation, maintainability, and the ability to extend the application.

RaceDayMVC is an ASP.NET Core MVC web application developed as part of the RaceDay Portfolio of Evidence (POE).

The platform supports the management of running, walking, and cycling events in South Africa. It provides interfaces for Event Organisers and Participants.

The MVC application communicates with the RaceDay RESTful API developed in Part 2.

Project Objectives

The objectives of RaceDay are to provide a user-friendly event management platform, support user authentication, allow organisers to manage events, and enable participants to browse events, register, and view their results.

Technologies Used

ASP.NET Core MVC

C#

ASP.NET Core Web API

Entity Framework Core

Microsoft SQL Server

HTML, CSS and Bootstrap

Swagger UI

Git and GitHub

Visual Studio

System Architecture

RaceDay uses a client-server architecture.

The MVC application provides the user interface and sends HTTP requests to the RESTful API.

The API handles business logic, authentication, and database operations using Entity Framework Core.

Microsoft SQL Server stores application data.

User Roles

Organiser

Organisers are responsible for creating and managing sporting events.

The system supports organiser-specific access to event management functionality.

Participant

Participants can create accounts, log in, browse events, access event registrations, and view race results through the available interfaces.

Authentication and Security

RaceDay uses session-based authentication.

Passwords are hashed before being stored in the database.

The API validates login credentials and establishes sessions for authenticated users.

Role-based checks restrict selected operations to authorised users.

Event Management

The RaceDay API provides endpoints for creating, viewing, updating, and deleting events.

Event information includes the event name, description, date, location, entry fee, and status.

Event Registrations

The platform includes registration functionality that allows participants to enrol in sporting events.

Registration records are stored in the SQL Server database.

Race Results

The RaceDay API supports recording and retrieving race results.

Results can include finishing times, finishing positions, and result statuses.

Image Upload Functionality

The API includes image upload endpoints for profile pictures and event banners.

Uploaded images are stored in the application's wwwroot/uploads directory.

The corresponding image paths are saved in the SQL Server database.

Supported image formats include JPG, JPEG, PNG, and WEBP.

The maximum accepted image size is 5 MB.

Database

The application uses a SQL Server database named RaceDayDB.

The database includes tables for users, participant profiles, events, categories, registrations, results, routes, and related event information.

Entity Framework Core is used for database access.

Installation Requirements

To run the project, install:

Visual Studio with ASP.NET development support

.NET SDK compatible with the project

SQL Server Express

SQL Server Management Studio

Running the Application

Open the TestingPoe API solution in Visual Studio.

Configure the database connection string.

Ensure the RaceDayDB database is available.

Start the API and confirm that Swagger loads.

Open the RaceDayMVC solution.

Confirm that the configured API base URL is correct.

Start the MVC application.

Register or log in using an appropriate user account.

Testing

The following functionality has been tested during development:

API compilation

Swagger API documentation loading

User login

Profile picture upload

Event banner upload

Image URL accessibility

Database image path persistence

MVC login and registration page access

Participant dashboard navigation links

Additional functionality should be tested before production deployment.

Known Limitations

The current image upload implementation stores files locally rather than in Azure Blob Storage.

Some MVC workflows may require additional integration testing.

The project should be considered an academic demonstration rather than a production deployment.

Version Control

Git and GitHub are used to manage source code changes.

Commits document implementation progress, feature additions, bug fixes, and documentation updates.
Conclusion

RaceDayMVC demonstrates the integration of an ASP.NET Core MVC application with a RESTful API and SQL Server database.

The project applies software development principles including separation of concerns, authentication, role-based functionality, database integration, and web application development.
