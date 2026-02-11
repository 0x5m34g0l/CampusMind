## Development Log
# MAUI + SQL Server problem

Issue:
MAUI project could not connect to SQL Server using SqlClient.

Cause:
MAUI runs cross-platform and cannot safely depend on Windows-only database drivers inside the UI layer.

Fix:
Separated the project into layers:

- UI (MAUI)

- Logic (Pure .NET Library)

- Data (Pure .NET Library)

Only Data layer communicates with SQL Server.

# Database configuration file not found

Issue:
dbsettings.json was not detected at runtime.

Cause:
MAUI executes from bin folder, not project folder.

Fix:

Set file property:

Copy to Output Directory → Copy if newer

# SQL Login failed

Error: Cannot open database requested by the login.

Cause:
Windows account was not mapped to database user.

Fix:
I just used the connection string in the right way. I put the correct name of the server, ex: "Server=CorrectName;..." instead of "Server=.;..."
