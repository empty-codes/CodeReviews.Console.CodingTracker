# Coding Tracker


## Challenges Faced and Lessons Learned

- **Adding 'System.' assembly references:** I thought they were automatically referenced but apparently in .NET Core and .NET 5+, for example the System.Configuration.ConfigurationManager NuGet package is used instead of directly adding a reference to the System.Configuration.dll
- Used 'conn.Execute(insertQuery, new {StartTime = session.StartTime, EndTime = session.EndTime, Duration = session.Duration})' instead of just 'conn.Execute(insertQuery, session) without knowing that when you pass the session object to the Execute method, Dapper automatically maps the properties of CodingSession (StartTime, EndTime, and Duration) to these parameter names, using reflection so as long as the property names in the CodingSession class and the parameter names in the SQL query match, Dapper will correctly bind the property values to the parameters.
- You cannot use a while loop directly inside a class: In C#, control structures like while loops must be inside methods, constructors, or properties, not directly within the class body.