-- Supply Chain Management - Clean Database Setup
-- This script initializes the database schema and essential lookup data.

-- 0. Cleanup existing tables (Drop in reverse dependency order)
IF OBJECT_ID('Notification', 'U') IS NOT NULL DROP TABLE Notification;
IF OBJECT_ID('Feedback', 'U') IS NOT NULL DROP TABLE Feedback;
IF OBJECT_ID('Payment', 'U') IS NOT NULL DROP TABLE Payment;
IF OBJECT_ID('Shipment', 'U') IS NOT NULL DROP TABLE Shipment;
IF OBJECT_ID('Inventory', 'U') IS NOT NULL DROP TABLE Inventory;
IF OBJECT_ID('Order_Details', 'U') IS NOT NULL DROP TABLE Order_Details;
IF OBJECT_ID('Purchase_Order', 'U') IS NOT NULL DROP TABLE Purchase_Order;
IF OBJECT_ID('Warehouse', 'U') IS NOT NULL DROP TABLE Warehouse;
IF OBJECT_ID('Customer', 'U') IS NOT NULL DROP TABLE Customer;
IF OBJECT_ID('Product', 'U') IS NOT NULL DROP TABLE Product;
IF OBJECT_ID('Supplier', 'U') IS NOT NULL DROP TABLE Supplier;
IF OBJECT_ID('Category', 'U') IS NOT NULL DROP TABLE Category;
IF OBJECT_ID('[User]', 'U') IS NOT NULL DROP TABLE [User];
IF OBJECT_ID('Role', 'U') IS NOT NULL DROP TABLE Role;

-- 1. Create Tables
CREATE TABLE Role (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Role(RoleID),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Contact_Info NVARCHAR(MAX),
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Address NVARCHAR(MAX),
    LeadTimeDays INT DEFAULT 5,
    Rating DECIMAL(3, 2) DEFAULT 0.0
);

CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Product_Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Category(CategoryID),
    SupplierID INT FOREIGN KEY REFERENCES Supplier(SupplierID)
);

CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(MAX),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Warehouse (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Location NVARCHAR(MAX),
    Capacity INT
);

CREATE TABLE Purchase_Order (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Pending',
    TotalAmount DECIMAL(18, 2) DEFAULT 0
);

CREATE TABLE Order_Details (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Purchase_Order(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Product(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL
);

CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT FOREIGN KEY REFERENCES Product(ProductID),
    WarehouseID INT FOREIGN KEY REFERENCES Warehouse(WarehouseID),
    Quantity_Available INT NOT NULL DEFAULT 0,
    Reorder_Level INT DEFAULT 10,
    LastUpdated DATETIME DEFAULT GETDATE()
);

CREATE TABLE Shipment (
    ShipmentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Purchase_Order(OrderID),
    WarehouseID INT FOREIGN KEY REFERENCES Warehouse(WarehouseID),
    Shipment_Date DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'In Transit',
    Shipped_Via NVARCHAR(100)
);

CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Purchase_Order(OrderID),
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50),
    Status NVARCHAR(50) DEFAULT 'Completed'
);

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    Rating INT,
    Comment NVARCHAR(MAX),
    FeedbackDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Notification (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES [User](UserID),
    Message NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0
);

-- 2. Insert Essential Lookup Data
INSERT INTO Role (RoleName) VALUES ('Admin'), ('Operator'), ('Customer'), ('Supplier');

INSERT INTO Category (Name) VALUES ('Industrial Parts'), ('Consumer Electronics'), ('Construction Materials'), ('Logistics Services');

-- 3. Create Default Admin User
-- Password is 'Admin@123' (Unsalted for demonstration/student style)
INSERT INTO [User] (Username, Email, Password, RoleID) VALUES
('admin', 'admin@supplychain.com', 'Admin@123', 1);
