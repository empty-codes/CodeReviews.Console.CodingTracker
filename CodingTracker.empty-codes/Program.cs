using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Data.Sqlite;

string connectionString = ConfigurationManager.ConnectionStrings["CodingSessionDb"].ConnectionString;

string dbPath = ConfigurationManager.AppSettings["DatabasePath"];
string dateFormat = ConfigurationManager.AppSettings["DateFormat"];

