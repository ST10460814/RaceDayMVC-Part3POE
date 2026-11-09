# RaceDayMVC- RaceDay Event Management Platform
## MVC Design Pattern
RaceDayMVC follows the Model-View-Controller architectural pattern. Models represent application data, Views provide the user interface, and Controllers process requests and coordinate interactions with the RESTful API.
This separation improves code organisation, maintainability, and the ability to extend the application.

## View Models

View models help transfer structured information between controllers and Razor views.

The EventViewModel is used by the MVC application when displaying, creating, and editing event information.

View models provide a clear structure for information required by the user interface.

## Razor Views

RaceDayMVC uses Razor views to generate HTML pages from server-side application data.

Razor syntax allows C# expressions to be combined with HTML, enabling pages to display dynamic information.

The application uses Razor views for authentication, dashboard navigation, and event-related interfaces.

## Bootstrap Styling

The RaceDayMVC interface uses Bootstrap for page layout and visual components.

Bootstrap containers, rows, columns, cards, and navigation elements provide a consistent design across the application.

The responsive grid system helps the interface adapt to different screen widths.

## Participant Dashboard Design

The Participant Dashboard provides a central navigation page for authenticated participants.

It contains three main areas: Browse Events, My Registrations, and My Results.

Navigation links direct participants to the corresponding MVC routes, making it easier to access the platform's main functions.

## Role-Based Navigation

RaceDay distinguishes between Organiser and Participant accounts.

After authentication, the application can use the stored role to determine which dashboard and navigation options should be displayed.

Server-side role checks are also necessary because hiding a link does not prevent someone from requesting a restricted endpoint directly.

## Password Protection

The TestingPoe API uses ASP.NET Core PasswordHasher to hash passwords during account registration.

During login, the entered password is checked against the stored hash using VerifyHashedPassword.

This approach avoids storing passwords as readable plain text in the database.

## Authentication Sessions

The API establishes a session after successful authentication and stores information such as UserId and Role.

Protected operations can inspect this session information to identify the authenticated user and apply access restrictions.

The logout endpoint clears the API session. The MVC application must also clear its own session when the user logs out.

## Event Ownership and Authorisation

Event ownership helps prevent organisers from modifying resources belonging to other organisers.

The image upload controller checks that the authenticated user has the Organiser role and that the event's OrganiserId matches the logged-in user's ID.

If these conditions are not met, the API rejects the request.

## Image File Validation

The image upload functionality checks file size and supported extensions before saving an uploaded image.

The API also inspects the file header to verify that the uploaded content matches an accepted image signature.

Uploaded files receive generated unique filenames to reduce filename collisions and avoid relying on user-provided filenames.


## Local Image Storage Process

When a valid image is uploaded, the API creates the uploads directory if necessary and saves the file in wwwroot/uploads.

The API returns a relative image URL, which is stored in the appropriate database record.

For profile images, the path is saved in ProfilePictureUrl. For event banners, the path is saved in BannerUrl.

This implementation uses local file storage rather than Azure Blob Storage.


## Database Relationships

RaceDayDB contains related tables representing users, events, categories, registrations, and results.

The Event table references an organiser, while registrations associate participants with event participation records.

Primary keys identify individual records, and foreign keys help maintain relationships between related tables.


## Database Constraints and Integrity

Database constraints support the consistency of stored RaceDay information.

Examples include unique values for selected fields, foreign-key relationships, and checks that restrict invalid status values or numerical values.

These constraints complement application-level validation and reduce the risk of inconsistent records.


## Testing Evidence and Verification

During development, the API was compiled successfully and Swagger was used to test selected endpoints.

Successful tests included authentication, profile image upload, event banner upload, and retrieval of an uploaded image using its URL.

SQL Server queries confirmed that uploaded image paths were stored in the appropriate records. The MVC Participant Dashboard and its navigation links were also inspected.

Further testing is required to confirm all participant and organiser workflows end-to-end.



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

## Application Configuration

The application uses configuration files such as appsettings.json and appsettings.Development.json.

Configuration includes settings for the database connection and the API endpoint used by the MVC application.

Developers should verify these settings before running the application on another computer. Passwords, access keys, and other secrets should not be committed to GitHub

## API Error Handling

The MVC EventController checks whether API responses indicate success before processing returned information.

When an API request fails, the application can display an error message or return an appropriate response instead of assuming the operation succeeded.

This helps users understand when information could not be loaded or an operation was unsuccessful.

## JSON Data Processing

RaceDay uses JSON as a data exchange format between the MVC application and the RESTful API.

The MVC EventController uses System.Text.Json to deserialize API responses into EventViewModel objects.

Case-insensitive property matching helps map JSON properties to their corresponding C# model properties.
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
