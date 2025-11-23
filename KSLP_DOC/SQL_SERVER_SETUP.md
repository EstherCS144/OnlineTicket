# SQL Server Setup Instructions

The OnlineTicket application has been converted to use SQL Server with Entity Framework Core.

## Setting Up SQL Server Connection

### Option 1: Azure SQL Database (Recommended)
1. Create an Azure SQL Database instance
2. Get your connection string from Azure Portal
3. Set it as the DATABASE_URL environment variable

### Option 2: Local SQL Server or Docker
If you have SQL Server installed locally or via Docker:
```
Server=your-server;Database=OnlineTicket;User Id=sa;Password=your-password;Encrypt=false;
```

## Connection String Format
SQL Server connection strings follow this format:
```
Server=<server-address>;Database=<database-name>;User Id=<username>;Password=<password>;Encrypt=false;TrustServerCertificate=true;
```

Example connection string:
```
Server=tcp:myserver.database.windows.net,1433;Initial Catalog=OnlineTicket;Persist Security Info=False;User ID=adminuser;Password=YourPassword123!;Encrypt=True;Connection Timeout=30;
```

## Database Creation
The application will automatically:
1. Create the database schema on first run
2. Seed demo data (3 organizers, 4 customers, 4 events, etc.)
3. Create admin account: admin@gmail.com / Admin123!@#

## Supported SQL Server Versions
- SQL Server 2019 and later
- Azure SQL Database
- SQL Server on Docker

## Troubleshooting

### Connection Timeout
- Check that your server is accessible
- Verify firewall rules allow your IP
- For Azure SQL, add your IP to the firewall rules

### Authentication Issues
- Verify username and password
- Check User ID format (may need domain\username for Windows auth)
- Try using SQL authentication instead of Windows auth

### Database Not Found
- The application will create the database automatically
- Ensure the user has CREATE DATABASE permissions

## Next Steps
1. Set up your SQL Server instance
2. Copy the connection string
3. Add it as DATABASE_URL secret in Replit
4. Restart the application
5. Login with admin@gmail.com / Admin123!@#
