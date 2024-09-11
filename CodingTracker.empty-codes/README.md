# Coding Tracker

## Overview

The Coding Tracker is a console application designed to log and manage coding sessions. Users can add, update, view, and delete coding sessions, as well as view reports based on their recorded data. The application utilizes a SQLite database for data storage and Dapper ORM for data access.

## Requirements

### Functional Requirements
- **Add Coding Session**: Users can add new coding sessions with start and end times.
- **Update Coding Session**: Users can update existing coding sessions.
- **View All Sessions**: Users can view all recorded coding sessions.
- **Delete Coding Session**: Users can delete specific coding sessions.
- **View Report**: Users can view a tailored report of their sessions (currently not implemented).

### Technical Requirements
- **Database**: SQLite
- **ORM**: Dapper
- **Configuration**: `app.config` for database connection and date format settings

## Features

- **CRUD Operations**: Create, Read, Update, and Delete operations for coding sessions.
- **Date and Time Validation**: Ensures correct date and time format and logical consistency (end time must be after start time).
- **Menu-driven Interface**: Provides a user-friendly console menu for interacting with the application.
- **Error Handling**: Handles exceptions and provides feedback for incorrect inputs or operations.

## Challenges Faced

- **Adding 'System.' assembly references:** I thought they were automatically referenced but apparently in .NET Core and .NET 5+, for example the System.Configuration.ConfigurationManager NuGet package is used instead of directly adding a reference to the System.Configuration.dll
- **Used 'conn.Execute(insertQuery, new {StartTime = session.StartTime, .....})**' instead of just 'conn.Execute(insertQuery, session) without knowing that when you pass the session object to the Execute method, Dapper automatically maps the properties of CodingSession (StartTime, EndTime, and Duration) to these parameter names, using reflection so as long as the property names in the CodingSession class and the parameter names in the SQL query match, Dapper will correctly bind the property values to the parameters.
- **You cannot use a while loop directly inside a class:** In C#, control structures like while loops must be inside methods, constructors, or properties, not directly within the class body.

## Lessons Learned

1. **Dapper Integration**: Using Dapper for ORM was efficient for simple CRUD operations but required careful handling of parameter names and types.
2. **Error Handling**: Proper error handling and feedback mechanisms are essential for a good user experience and debugging.
3. **Date Formatting**: Consistent date formatting between the application and database is crucial for data integrity and avoiding parsing errors.

## Areas for Improvement

1. **User Interface**: Enhance the console UI for better user experience, possibly with more detailed prompts and error messages.
2. **Report Generation**: Implement the tailored report feature to provide insights and summaries based on recorded sessions.
3. **Testing**: Add unit tests and integration tests to ensure the reliability of the codebase.

## Resources Used

- **Dapper**: [Dapper Documentation](https://github.com/DapperLib/Dapper)
- **Configuration Management**: [ConfigurationManager Class](https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager)
