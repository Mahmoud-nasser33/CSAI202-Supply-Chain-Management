# Silsila SCM | Enterprise Supply Chain Command Center 🚀

![Supply Chain Branding](SupplyChain.Frontend/SupplyChain.Frontend/wwwroot/images/logistics-ship.png)

Welcome to **Silsila SCM**, a state-of-the-art, cinematic Supply Chain Management system built to provide global visibility, predictive intelligence, and seamless operational control.

## ✨ Features
- **Cinematic UI/UX**: Professional enterprise aesthetics with glassmorphism, fluid animations, and high-quality industrial imagery.
- **Strategic Analytics**: Real-time monitoring of revenue, inventory health, and AI-driven churn risk modeling.
- **Dynamic Operations**: Management of Orders, Shipments, Warehouses, and Suppliers with live data integration.
- **Predictive AI Insights**: Context-aware recommendations for logistics optimization and inventory advisory.
- **Role-Based Governance**: Secure access control for Admin, Staff, Customers, and Suppliers.

---

## 🛠️ Tech Stack
- **Frontend**: ASP.NET Core Razor Pages, Bootstrap 5, Vanilla CSS, JavaScript (ES6+).
- **Backend**: ASP.NET Core Web API, Entity Framework Core.
- **Intelligence**: Integrated AI logic for simulated predictive insights.
- **Database**: SQL Server (LocalDB / Azure SQL).

---

## 🔌 Database Connectivity Guide

### 1. Connection String Configuration
Navigate to `SupplyChainSystem\SupplyChain.Backend\appsettings.json` and update the `DefaultConnection` string with your server details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SupplyChainDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Schema Setup
Run the provided SQL scripts in the following order using SQL Server Management Studio (SSMS) or Azure Data Studio:
1. `SQL_1_CREATE_SCHEMA.sql`
2. `SQL_2_INSERT_DATA.sql`

Alternatively, use `SQL_CLEAN_SETUP.sql` for a fresh installation.

### ❓ Why are my numbers showing as "0"?
If the dashboard or pages display **0** for statistics (e.g., 0 Customers, 0 Orders):
- **Unconnected Database**: The frontend is gracefully handling the absence of data. Ensure the Backend API is running and successfully connected to the SQL database.
- **Empty Tables**: If the scripts were not run, there is no data to aggregate. Please ensure the `INSERT_DATA` script has been executed.

---

## 🚀 Getting Started

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Mahmoud-nasser33/CSAI202-Supply-Chain-Management.git
   ```
2. **Launch the Backend**:
   ```bash
   cd SupplyChainSystem/SupplyChain.Backend
   dotnet run
   ```
3. **Launch the Frontend**:
   ```bash
   cd SupplyChain.Frontend/SupplyChain.Frontend
   dotnet run
   ```

---

## 📌 Note on Project Status
> [!IMPORTANT]  
> This is **NOT the final version**. This codebase represents an active development milestone focusing on UI refinement and cinematic asset integration.  
> **A new, enhanced version of Silsila SCM is coming soon!** 🌟

---

## 📄 License
This project is licensed under the MIT License - see the `LICENSE` file for details.

© 2025 Silsila SCM | Strategic Supply Chain Intelligence.
