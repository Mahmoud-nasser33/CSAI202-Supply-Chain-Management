USE SupplyChain;
GO

INSERT INTO [Role] (Role_Name, Permissions) VALUES
('Admin', 'Full Access'),
('Customer', 'View Orders, Place Order'),
('Supplier', 'Manage Products, View Orders'),
('Warehouse Manager', 'Manage Inventory'),
('Guest', 'View Products Only');
GO

SET IDENTITY_INSERT [User] ON;
INSERT INTO [User] (UserID, User_Name, Email, Password, RoleID) VALUES
(1, 'Admin', 'admin@supply.com', 'admin13', 1),
(2, 'Ahmed Mohamed', 'ahmed@gmail.com', 'pass1023', 2),
(3, 'Sara Ali', 'sara@yahoo.com', 'pawwd123', 2),
(4, 'Omar Khaled', 'omar@supplier.com', 'pass1997', 3),
(5,'Mohamed Tamer', 'mo.tamer3655@gmail.com', 'mo555', 2),
(6, 'Fatma Hassan', 'fatma@gmail.com', 'pass117', 2);
SET IDENTITY_INSERT [User] OFF;
GO

SET IDENTITY_INSERT Customer ON;
INSERT INTO Customer (CustomerID, Name, UserID) VALUES
(1, 'Ahmed Mohamed', 2),
(2, 'Sara Ali', 3),
(3, 'Fatma Hassan', 6);
SET IDENTITY_INSERT Customer OFF;
GO

SET IDENTITY_INSERT Supplier ON;
INSERT INTO Supplier (SupplierID, Name, Contact_Info, Address, UserID) VALUES
(1, 'Tech Suppliers Co', '01001234567', 'Cairo', 5),
(2, 'Global Electronics', '01119876543', 'Alexandria', NULL),
(3, 'Smart Devices Ltd', '01552345678', 'Cairo', NULL);
SET IDENTITY_INSERT Supplier OFF;
GO

INSERT INTO Category (Name, Description) VALUES
('Electronics', 'Phones, Laptops, Accessories'),
('Home Appliances', 'Fridges, Washers, AC'),
('Fashion', 'Clothes & Shoes'),
('Books', 'Educational & Novels');
GO

INSERT INTO Product (Product_Name, Description, Price, CategoryID) VALUES
('iPhone 15 Pro', 'Latest Apple smartphone', 45999.00, 1),
('Samsung Galaxy S24', 'Flagship Android', 38999.00, 1),
('MacBook Pro 16"', 'M3 Pro Chip', 89999.00, 1),
('Dell XPS 13', 'Ultra slim laptop', 52999.00, 1),
('Sony WH-1000XM5', 'Noise cancelling headphones', 14999.00, 1),
('LG OLED 55"', '4K Smart TV', 38999.00, 2),
('Samsung Fridge 600L', 'Twin cooling', 45999.00, 2),
('Toshiba Washing Machine 10kg', 'Inverter', 18999.00, 2),
('Nike Air Max', 'Running shoes', 5499.00, 3),
('Rich Dad Poor Dad', 'Finance book', 299.00, 4);

INSERT INTO Warehouse (Name, Location) VALUES
('Main Cairo Warehouse', '6th of October'),
('Alex Branch', 'Smouha'),
('Delta Storage', 'Mansoura');
GO

INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available, Reorder_Level) VALUES
(1, 1, 15, 5),
(1, 2, 8, 5),
(2, 1, 3, 10),
(3, 1, 25, 8),
(5, 1, 0, 20);
GO

INSERT INTO Purchase_Order (CustomerID, OrderDate, Status, TotalAmount) VALUES
(1, '2025-11-20', 'Delivered', 125999.00),
(2, '2025-12-01', 'Shipped', 56998.00),
(1, '2025-12-05', 'Pending', 45999.00);
GO

INSERT INTO Order_Details (OrderID, ProductID, Quantity, UnitPrice) VALUES
(4, 1, 1, 45999.00),
(2, 5, 2, 14999.00),
(2, 9, 1, 5499.00),
(3, 2, 1, 41999.00),
(2, 6, 1, 14999.00),
(3, 3, 1, 52999.00),
(4, 7, 1, 45999.00),
(2, 4, 1, 64999.00),
(3, 10, 3, 4999.00),
(2, 1, 2, 45999.00),
(4, 5, 1, 14999.00);
GO

INSERT INTO Payment (OrderID, Payment_Date, Payment_Method, Status, Amount) VALUES
(3, '2025-11-20', 'Credit Card', 'Completed', 125997.00),
(2, '2025-12-02', 'Cash on Delivery', 'Completed', 56998.00),
(3, '2025-12-10', 'Credit Card', 'Pending', 45999.00),
(4, '2025-12-09', 'Bank Transfer', 'Completed', 18999.00),
(2, '2025-12-11', 'Credit Card', 'Completed', 79996.00);
GO

INSERT INTO Shipment (OrderID, WarehouseID, Shipment_Date, Status, Shipped_Via) VALUES
(4, 1, '2025-11-21', 'Delivered', 'Aramex'),
(2, 1, '2025-12-03', 'In Transit', 'DHL'),
(3, 2, NULL, 'Processing', 'Aramex'),
(4, 1, '2025-12-10', 'Delivered', 'Local Courier'),
(2, 1, '2025-12-11', 'Shipped', 'FedEx');
GO

INSERT INTO Feedback (CustomerID, OrderID, Rating, Comment) VALUES
(1, 2, 5, 'Excellent and very fast delivery'),
(2, 2, 4, 'Very good but packaging could be better'),
(3, 3, NULL, NULL),
(3, 2, 5, 'The best service I have seen'),
(2, 3, 3, 'Unfortunately, the product arrived two days late');
GO

INSERT INTO Supplies (ProductID, SupplierID) VALUES
(1, 1), (1, 2),
(2, 1),
(3, 2),
(4, 1),
(5, 1), (5, 3),
(6, 2),
(7, 1),
(8, 2),
(9, 3),
(10, 1);
GO

INSERT INTO Notification (UserID, Message, Notification_Type) VALUES
(1, 'Your order #4 has been accepted successfully', 'Order'),
(2, 'Your order #2 has been shipped', 'Shipment'),
(3, 'You have a new order #6 awaiting processing', 'Order'),
(5, 'New stock added for iPhone 15', 'Stock'),
(1, 'Received 5-star rating from Ahmed Mohamed', 'Feedback'),
(4, 'New purchase order added from customer Sara', 'Order'),
(2, '10% discount activated on upcoming orders', 'Promotion');
GO
