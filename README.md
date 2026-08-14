# Silsila Supply

> **What this project is:** a **database-focused** project for **CSAI 202**. The point is to design and test the **SQL Server / LocalDB database** — schema, constraints, seed data, parameterized queries, transactions and delete cascades — not to build a full-stack website. The web UI is only a thin harness for exercising the database during testing.

It is built with **ASP.NET Core Razor Pages** on top of the database via plain **ADO.NET**.

## Running the project

1. Create the database by running `setup_database.sql` against `(localdb)\MSSQLLocalDB`:

   ```sh
   sqlcmd -S "(localdb)\MSSQLLocalDB" -i setup_database.sql
   ```

   The script is **re-runnable** — it creates `SupplyChainDB` if missing, then drops and recreates every table and reloads the seed data. It is destructive: re-running it wipes all data.
2. Open `SilsilaSupply.sln` in Visual Studio and run, or:

   ```sh
   dotnet run --project src/SilsilaSupply
   ```

The connection string is defined in `appsettings.json` (`ConnectionStrings:DefaultConnection`) and uses Windows-integrated auth against LocalDB — no credentials are stored in source. In a real deployment the connection string should be supplied through environment variables or user secrets instead.

## Architecture

The web app is a single Razor Pages project, used here mainly as a testing harness for the database. PageModels are thin: they bind and validate form input, then delegate all database work to services.

```text
Razor Pages
    ↓
PageModels        (binding, validation, deciding the result to return)
    ↓
Services          (connections, parameterized SQL, mapping, transactions, DB error handling)
    ↓
ADO.NET
    ↓
SQL Server
```

### Services

Each service in `Services/` owns one entity's database access:

- `CustomerService`, `OrderService`, `ProductService`, `InventoryService`, `ShipmentService`, `SupplierService`
- `WarehouseService` — warehouse options used by the Inventory and Shipment forms
- `DashboardService` — dashboard statistics

Services are registered through dependency injection in `Program.cs` as scoped, concrete classes (no interface layer — the project is too small for it to add value).

### How the layers behave

- **Reads** return `DataResult<T>`. **Writes** return `OperationResult`. Both carry a user-facing error message on failure.
- All SQL is parameterized. No user input is ever concatenated into a query.
- Multi-statement operations (customer, order, product and supplier deletes) run inside a single `SqlTransaction` and roll back on any failure — a partial delete can never be left behind. Deleting a record that no longer exists reports a friendly "no longer exists" message instead of silent success.
- Database exceptions are caught at the service boundary, logged with technical detail, and mapped to a friendly message. Known cases:
  - duplicate key (e.g. a customer/supplier email that already exists) → "already exists"
  - foreign key violation → "still referenced"
- `SqlDataReader` values are mapped with null-safe helpers (`SqlReaderExtensions`), and every nullable column in the schema is handled explicitly.
- Forms use Data Annotations + `ModelState.IsValid`. Invalid submissions re-render the page with visible validation errors — invalid values are never silently coerced to `0`.
- **Inventory is unique per product + warehouse** (enforced by a `UNIQUE (ProductID, WarehouseID)` constraint). Recording stock for an existing pair *adds to* the current quantity inside a transaction rather than creating a duplicate row; the edit page sets the exact quantity.
- The Inventory page supports a **low-stock-only filter** (`/Inventory?low=1`), linked from the dashboard's Low stock stat. It uses the same rule as the dashboard count: quantity below the reorder level.

## Navigation

Dashboard · Orders · Shipments · Products · Inventory · Suppliers · Customers

## CRUD coverage

Every entity has a create form on its list page, an **Edit** page (`/Customers/Edit/{id}`, etc.), and a remove action:

| Entity | Page | Edit page |
|---|---|---|
| Customers | `/Customers` | `/Customers/Edit/{id}` |
| Orders | `/Orders` | `/Orders/Edit/{id}` |
| Shipments | `/Shipments` | `/Shipments/Edit/{id}` |
| Products | `/Products` | `/Products/Edit/{id}` |
| Inventory | `/Inventory` | `/Inventory/Edit/{id}` |
| Suppliers | `/Suppliers` | `/Suppliers/Edit/{id}` |

## Known limitations and future scope

- **No authentication.** The application has no login, registration, or authorization. The `User`, `Role` and `Password` tables exist in the schema but are **not integrated**, and no authentication code is planned. The `Password` column is never exposed through the UI.
- **Payment, Feedback, Notification tables exist but have no pages.** They are part of the database schema but are not currently managed by the application. Payment and Feedback records are cleaned up correctly when their parent order/customer is deleted.
- **Single-user operations.** There are no roles or per-user permissions.
- **Seed data is in Egyptian Arabic.** Status values (e.g. `Tam El-Tasleem`) are stored as free text; the UI shows them as-is.
- **Currency is displayed as EGP (E£).** Amounts are stored as plain `DECIMAL(18,2)` in the schema with no currency column.

## Delete behavior

Removing a record cascades through its dependants inside a transaction:

| Deleted record | Also removes |
|---|---|
| Customer | their feedback, orders, order line items, shipments, payments |
| Order | order line items, shipments, payments |
| Product | its inventory entries and order line items |
| Supplier | its products, and each product's inventory entries and order line items |
| Inventory / Shipment | nothing (leaf records) |
