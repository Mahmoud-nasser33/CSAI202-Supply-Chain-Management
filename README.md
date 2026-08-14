# Silsila Supply

A database project. The focus is the SQL Server / LocalDB database — schema, constraints, seed data, parameterized queries, transactions, and delete cascades. The web UI is just a simple harness for testing the database, not the main point of the project. It's built with ASP.NET Core Razor Pages and plain ADO.NET.

To run it, run `setup_database.sql` against `(localdb)\MSSQLLocalDB` to create the database and load seed data. Then open `SilsilaSupply.sln` in Visual Studio and run it, or use `dotnet run --project src/SilsilaSupply` from the command line.

The app follows a simple flow: Razor Pages call into PageModels, which handle form binding and validation, and PageModels delegate all database work to Services, which handle connections, parameterized queries, transactions, and error handling.

The project supports full CRUD for Customers, Orders, Shipments, Products, Inventory, and Suppliers, with cascading deletes wrapped in transactions and parameterized SQL throughout. Common database errors like duplicates or foreign key conflicts are turned into friendly messages, and the Inventory page includes a low-stock filter.

There's no authentication and the app is single-user only. The Payment, Feedback, and Notification tables exist in the schema but don't have pages yet.



