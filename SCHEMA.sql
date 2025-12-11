CREATE DATABASE SupplyChain;
USE SupplyChain;

CREATE TABLE [Role] (
  [RoleID] INT PRIMARY KEY IDENTITY(1,1),
  [Role_Name] NVARCHAR(255) NOT NULL,
  [Permissions] NVARCHAR(255)
);
GO

CREATE TABLE [User] (
  [UserID] INT PRIMARY KEY IDENTITY(1,1),
  [User_Name] NVARCHAR(255) NOT NULL,
  [Email] NVARCHAR(255) UNIQUE NOT NULL,
  [Password] NVARCHAR(255) NOT NULL,
  [RoleID] INT,
  FOREIGN KEY (RoleID) REFERENCES [Role](RoleID)
);
GO

CREATE TABLE [Customer] (
  [CustomerID] INT PRIMARY KEY IDENTITY(1,1),
  [UserID] INT NOT NULL UNIQUE,
  [Name] NVARCHAR(255) NOT NULL,
  [Address] NVARCHAR(255),
  FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);
GO

CREATE TABLE [Category] (
  [CategoryID] INT PRIMARY KEY IDENTITY(1,1),
  [Name] NVARCHAR(255) NOT NULL,
  [Description] NVARCHAR(255)
);
GO

CREATE TABLE [Product] (
  [ProductID] INT PRIMARY KEY IDENTITY(1,1),
  [Product_Name] NVARCHAR(255) NOT NULL,
  [Description] NVARCHAR(255),
  [Price] DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
  [CategoryID] INT,
  FOREIGN KEY (CategoryID) REFERENCES [Category](CategoryID)
);
GO

CREATE TABLE [Warehouse] (
  [WarehouseID] INT PRIMARY KEY IDENTITY(1,1),
  [Name] NVARCHAR(255) NOT NULL,
  [Location] NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE [Inventory] (
  [InventoryID] INT PRIMARY KEY IDENTITY(1,1),
  [ProductID] INT NOT NULL,
  [WarehouseID] INT NOT NULL,
  [Quantity_Available] INT NOT NULL CHECK (Quantity_Available >= 0),
  [Reorder_Level] INT CHECK (Reorder_Level >= 0),
  FOREIGN KEY (ProductID) REFERENCES [Product](ProductID),
  FOREIGN KEY (WarehouseID) REFERENCES [Warehouse](WarehouseID)
);
GO

CREATE TABLE [Supplier] (
  [SupplierID] INT PRIMARY KEY IDENTITY(1,1),
  [Name] NVARCHAR(255) NOT NULL,
  [Contact_Info] NVARCHAR(255),
  [Address] NVARCHAR(255),
  [UserID] INT,
  FOREIGN KEY (UserID) REFERENCES [User](UserID)
);
GO

CREATE TABLE [Purchase_Order] (
  [OrderID] INT PRIMARY KEY IDENTITY(1,1),
  [CustomerID] INT NOT NULL,
  [OrderDate] DATE NOT NULL,
  [Status] NVARCHAR(255) NOT NULL,
  [TotalAmount] DECIMAL(10,2) CHECK (TotalAmount >= 0),
  FOREIGN KEY (CustomerID) REFERENCES [Customer](CustomerID)
);
GO

CREATE TABLE [Order_Details] (
  [OrderID] INT NOT NULL,
  [ProductID] INT NOT NULL,
  [Quantity] INT NOT NULL CHECK (Quantity > 0),
  [UnitPrice] DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),
  CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderID, ProductID),
  FOREIGN KEY (OrderID) REFERENCES [Purchase_Order](OrderID) ON DELETE CASCADE,
  FOREIGN KEY (ProductID) REFERENCES [Product](ProductID)
);
GO

CREATE TABLE [Payment] (
  [PaymentID] INT PRIMARY KEY IDENTITY(1,1),
  [OrderID] INT NOT NULL,
  [Payment_Date] DATE NOT NULL,
  [Payment_Method] NVARCHAR(255),
  [Status] NVARCHAR(255),
  [Amount] DECIMAL(10,2) CHECK (Amount >= 0),
  FOREIGN KEY (OrderID) REFERENCES [Purchase_Order](OrderID) ON DELETE CASCADE
);
GO

CREATE TABLE [Shipment] (
  [ShipmentID] INT PRIMARY KEY IDENTITY(1,1),
  [OrderID] INT NOT NULL,
  [WarehouseID] INT NOT NULL,
  [Shipment_Date] DATE,
  [Status] NVARCHAR(255),
  [Shipped_Via] NVARCHAR(255),
  FOREIGN KEY (OrderID) REFERENCES [Purchase_Order](OrderID) ON DELETE CASCADE,
  FOREIGN KEY (WarehouseID) REFERENCES [Warehouse](WarehouseID)
);
GO

CREATE TABLE [Notification] (
  [NotificationID] INT PRIMARY KEY IDENTITY(1,1),
  [UserID] INT NOT NULL,
  [Message] NVARCHAR(255) NOT NULL,
  [Notification_Type] NVARCHAR(255),
  FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);
GO

CREATE TABLE [Feedback] (
  [FeedbackID] INT PRIMARY KEY IDENTITY(1,1),
  [CustomerID] INT NOT NULL,
  [OrderID] INT NOT NULL,
  [Rating] INT CHECK (Rating BETWEEN 1 AND 5),
  [Comment] NVARCHAR(255),
  FOREIGN KEY (CustomerID) REFERENCES [Customer](CustomerID) ON DELETE CASCADE,
  FOREIGN KEY (OrderID) REFERENCES [Purchase_Order](OrderID) ON DELETE CASCADE
);
GO

CREATE TABLE [Supplies] (
  [ProductID] INT NOT NULL,
  [SupplierID] INT NOT NULL,
  CONSTRAINT PK_Supplies PRIMARY KEY (ProductID, SupplierID),
  FOREIGN KEY (ProductID) REFERENCES [Product](ProductID) ON DELETE CASCADE,
  FOREIGN KEY (SupplierID) REFERENCES [Supplier](SupplierID)
);
GO
