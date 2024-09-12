# Coding Tracker

## Overview

The Coding Tracker is a console application designed to log and manage coding sessions. Users can add, update, view, and delete coding sessions, as well as view reports based on their recorded data. The application utilizes a SQLite database for data storage and Dapper ORM for data access.

![Main Menu UI](CodingTracker.empty-codes/Images/menu-ui.PNG)

## Requirements

### Functional Requirements
- **Add Coding Session**: Users can add new coding sessions with start and end times.
- **Update Coding Session**: Users can update existing coding sessions.
- **View All Sessions**: Users can view all recorded coding sessions.
- **Delete Coding Session**: Users can delete specific coding sessions.
- **View Report**: Users can view a tailored report of their sessions.

### Technical Requirements
- **Database**: SQLite
- **ORM**: Dapper
- **Configuration**: `app.config` for database connection and date format settings

## Features

- **CRUD Operations**: Create, Read, Update, and Delete operations for coding sessions.
- **Date and Time Validation**: Ensures correct date and time format and logical consistency (end time must be after start time).
- **Menu-driven Interface**: Provides a user-friendly console menu for interacting with the application.
- **Error Handling**: Handles exceptions and provides feedback for incorrect inputs or operations.

![Reports](CodingTracker.empty-codes/Images/report-ui.PNG)
![Goal Stats](CodingTracker.empty-codes/Images/goal-ui.PNG)

## Challenges Faced and Lessons Learned

- **Adding 'System.' assembly references:** I thought they were automatically referenced but apparently in .NET Core and .NET 5+, for example the System.Configuration.ConfigurationManager NuGet package is used instead of directly adding a reference to the System.Configuration.dll
- **Used 'conn.Execute(insertQuery, new {StartTime = session.StartTime, .....})**' instead of just 'conn.Execute(insertQuery, session) without knowing that when you pass the session object to the Execute method, Dapper automatically maps the properties of CodingSession (StartTime, EndTime, and Duration) to these parameter names, using reflection so as long as the property names in the CodingSession class and the parameter names in the SQL query match, Dapper will correctly bind the property values to the parameters.
- **You cannot use a while loop directly inside a class:** In C#, control structures like while loops must be inside methods, constructors, or properties, not directly within the class body.
- **Implementing Separation of Concerns, OOP, SOLID, etc:** Normally i would just code all my logic inside the program.cs using methods, maybe making one class to put the methods in and using the progam cs for user interfacing but now i had to break everything into separate classes so it took me more time to undestand how to do that, for example the stopwatch feature, normally i would put all the logic in the 'Usestopwatch' method in the userinput.cs but i decided to create a dedicated class for it instead so it took me a lot more time trying to figure out how to then connect this class with the existing classes.
- **Data Type to represent Time:** Was in a pickle whether to use DateTime/ TimeSpan/ ints/doubles to represent variables where i needed just hours instead of days; learnt it just depends tbh

## Areas for Improvement

1. **Input Experience:** Entering time and subsequently dates is kinda tedious; user input experience can be improved on.
2. **Goal Persistence:** Goals are not stored in the database, requiring re-entry each time. Implementing persistent goal storage would also enhance user experience.

## Resources Used

- **Dapper**: [Dapper Documentation](https://github.com/DapperLib/Dapper)
- **Configuration Management**: [ConfigurationManager Class](https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager)
- **Spectre Console**: [Spectre Console Documentation](https://spectreconsole.net/)
