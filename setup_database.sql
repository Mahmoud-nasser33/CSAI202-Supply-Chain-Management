
CREATE TABLE Role (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Role(RoleID),
    CreatedAt DATE
);

CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Contact_Info VARCHAR(255),
    Email VARCHAR(255) UNIQUE NOT NULL,
    Address VARCHAR(255),
    LeadTimeDays INT,
    Rating DECIMAL(3, 2)
);

CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Product_Name VARCHAR(255) NOT NULL,
    Description VARCHAR(255),
    Price DECIMAL(18, 2) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Category(CategoryID),
    SupplierID INT FOREIGN KEY REFERENCES Supplier(SupplierID)
);

CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Phone VARCHAR(20),
    Address VARCHAR(255),
    City VARCHAR(100),
    Country VARCHAR(100),
    CreatedAt DATE
);

CREATE TABLE Warehouse (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Location VARCHAR(255),
    Capacity INT
);

CREATE TABLE Purchase_Order (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    OrderDate DATE,
    Status VARCHAR(50),
    TotalAmount DECIMAL(18, 2)
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
    Quantity_Available INT NOT NULL,
    Reorder_Level INT,
    LastUpdated DATE
);

CREATE TABLE Shipment (
    ShipmentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Purchase_Order(OrderID),
    WarehouseID INT FOREIGN KEY REFERENCES Warehouse(WarehouseID),
    Shipment_Date DATE,
    Status VARCHAR(50),
    Shipped_Via VARCHAR(100)
);

CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Purchase_Order(OrderID),
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentDate DATE,
    PaymentMethod VARCHAR(50),
    Status VARCHAR(50)
);

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    Rating INT,
    Comment VARCHAR(255),
    FeedbackDate DATE
);

CREATE TABLE Notification (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES [User](UserID),
    Message VARCHAR(255) NOT NULL,
    CreatedAt DATE,
    IsRead INT
);

INSERT INTO Role (RoleName) VALUES ('Admin'), ('Operator'), ('Customer'), ('Supplier');

INSERT INTO Category (Name) VALUES ('Mawad Benaa'), ('Agheza Electronic'), ('Ghayiar'), ('Khadamat Logistics');

INSERT INTO [User] (Username, Email, Password, RoleID, CreatedAt) VALUES
('admin_main', 'admin@silsila.com', 'SecurePass123', 1, '2025-01-01'),
('warehouse_op', 'op1@silsila.com', 'Warehouse2024', 2, '2025-01-01');

INSERT INTO Supplier (Name, Email, Contact_Info, Address, LeadTimeDays, Rating) VALUES
('El-Nasr Import & Export', 'sales@elnasr.com', '+20 100 0000', 'Nasr City, Cairo', 7, 4.8),
('El-Masryeen Logistics', 'info@masryeen.com', '+20 120 0000', 'Alex Port Zone', 3, 4.5),
('Al-Ahram Steel Factory', 'sales@alahram.com', '+20 110 0000', 'Helwan Industrial City', 14, 4.2);

INSERT INTO Product (Product_Name, Description, Price, CategoryID, SupplierID) VALUES
('Mawaseer PVC 4 Inch', 'High durablity plastic pipes', 450.00, 1, 1),
('Shikara Asmant Assiut', 'Portland Cement 50kg', 12.50, 1, 3),
('Borta Electronic V3', 'Advanced circuit board', 85.00, 2, 1),
('Hadeed Ezz 12mm', 'Reinforced steel bars', 120.00, 1, 3),
('Router We', 'High speed internet router', 310.00, 2, 1);

INSERT INTO Customer (Name, Email, Phone, Address, City, Country, CreatedAt) VALUES
('Mahmoud Sakr', 'mahmoud@silsila.com', '+20 100 1234567', 'El-Tagamoa El-Khamis', 'Cairo', 'Egypt', '2025-01-02'),
('Mohamed Tamer', 'mohamed@silsila.com', '+20 111 7654321', 'Smouha', 'Alexandria', 'Egypt', '2025-01-02'),
('Ahmed Noman', 'ahmed@store.com', '+20 155 0001', 'El-Mohandessin', 'Giza', 'Egypt', '2025-01-03');

INSERT INTO Warehouse (Name, Location, Capacity) VALUES
('Makhzan El-Obour', 'Obour City Industrial Zone', 10000),
('Makhzan Alex Port', 'Alexandria Free Zone', 5000),
('Makhzan El-Sokhna', 'Ein Sokhna Port', 15000);

INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available, Reorder_Level, LastUpdated) VALUES
(1, 1, 50, 10, '2025-01-05'), (2, 2, 120, 20, '2025-01-05'), (3, 1, 400, 50, '2025-01-05'), (4, 3, 1000, 100, '2025-01-05'), (5, 2, 25, 5, '2025-01-05');

INSERT INTO Purchase_Order (CustomerID, OrderDate, TotalAmount, Status) VALUES
(1, '2023-12-01', 900.00, 'Tam El-Tasleem'),
(2, '2023-12-15', 310.00, 'Gari El-Tagheez'),
(3, '2025-01-06', 570.00, 'Mo3alaq');

INSERT INTO Order_Details (OrderID, ProductID, Quantity, UnitPrice) VALUES
(1, 1, 2, 450.00),
(2, 5, 1, 310.00),
(3, 2, 2, 85.00), (3, 1, 1, 450.00); 

INSERT INTO Shipment (OrderID, WarehouseID, Status, Shipped_Via, Shipment_Date) VALUES
(1, 1, 'Wasel', 'El-Bariq Express', '2025-01-04'),
(2, 2, 'Fe El-Tareeq', 'Arabia Trans', '2025-01-06');

INSERT INTO Payment (OrderID, Amount, PaymentMethod, Status, PaymentDate) VALUES
(1, 900.00, 'Visa Card', 'Tam El-Daf3', '2025-01-04'),
(3, 570.00, 'Tahweel Bank', 'Mo3alaq', '2025-01-06');

INSERT INTO Feedback (CustomerID, Rating, Comment, FeedbackDate) VALUES
(1, 5, 'Momtaz gedan, shokran!', '2025-01-07'),
(2, 4, 'El-shahn ataakhar shwaya bas el-product kwayes.', '2025-01-07');

INSERT INTO Notification (UserID, Message, IsRead, CreatedAt) VALUES
(1, 'Inventory Report shahr 12 gahez.', 0, '2025-01-08'),
(2, 'Tanbeeh: Product 5 stock olayel gedan.', 1, '2025-01-08');
