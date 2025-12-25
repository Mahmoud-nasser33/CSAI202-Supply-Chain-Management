# Database Setup and Connection Guide

This guide explains how to connect the Supply Chain Management application to your SQL Server database and initialize it with a clean starting state.

## 1. Prerequisites
- **SQL Server**: Ensure you have SQL Server (Express, LocalDB, or Full) installed.
- **SSMS or Visual Studio**: Any tool capable of running SQL scripts.

## 2. Initialize the Database
1. Open your SQL tool (e.g., SQL Server Management Studio).
2. Create a new database named `SupplyChain`.
3. Run the script provided in [SQL_CLEAN_SETUP.sql](file:///c:/Users/mahmoud/Desktop/CSAI202-Supply-Chain-Management-2/SQL_CLEAN_SETUP.sql).
   - This script will create all necessary tables.
   - It will insert basic roles (Admin, Supplier, etc.) and categories.
   - It creates a default admin user: `admin` / `Admin@123`.

## 3. Configure the Application
You need to update the connection string in the Backend project to point to your SQL Server.

1. Open [appsettings.json](file:///c:/Users/mahmoud/Desktop/CSAI202-Supply-Chain-Management-2/SupplyChainSystem/SupplyChain.Backend/appsettings.json).
2. Locate the `DefaultConnection` string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SupplyChain;Integrated Security=True;TrustServerCertificate=True;"
   }
   ```
3. Update the `Server` value if your SQL Server instance is different. Use `.` or `localhost` if using a local standard instance, or `(localdb)\MSSQLLocalDB` for Visual Studio's default local DB.

## 4. Launch and Verify
1. Start the Backend API (runs on Port 5000).
2. Start the Frontend Application (runs on Port 5001).
3. Log in with the default credentials:
   - **Username**: `admin`
   - **Password**: `Admin@123`
4. Verify that the pages (Products, Orders, Customers) are empty and ready for your own data entry.

> [!TIP]
> If you encounter a "Connection Error", double-check that the SQL Server service is running and that the `Database` name in the connection string matches the one you created.
