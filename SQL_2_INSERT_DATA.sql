
-- Insert Data

INSERT INTO Role (RoleName) VALUES ('Admin'), ('Operator'), ('Customer'), ('Supplier');

INSERT INTO Category (Name) VALUES ('Industrial Parts'), ('Consumer Electronics'), ('Construction Materials'), ('Logistics Services');

INSERT INTO [User] (Username, Email, Password, RoleID) VALUES
('admin_main', 'admin@silsila.com', 'SecurePass123', 1),
('warehouse_op', 'op1@silsila.com', 'Warehouse2024', 2);

INSERT INTO Supplier (Name, Email, Contact_Info, Address, LeadTimeDays, Rating) VALUES
('Global Steel Ltd', 'sales@globalsteel.com', '+49 123 456789', 'Main Industrial Zone, Berlin, Germany', 7, 4.8),
('Eco-Logistics', 'contact@eco.com', '+31 20 5550199', 'Logistics Park, Amsterdam, Netherlands', 3, 4.5),
('Titan Electronics', 'info@titanelec.com', '+1 415 5550123', 'Silicon Valley Blvd, CA, USA', 14, 4.2);

INSERT INTO Product (Product_Name, Description, Price, CategoryID, SupplierID) VALUES
('Steel Beam 20ft', 'High-strength structural steel', 450.00, 3, 1),
('Circuit Board V3', 'Advanced logic controller', 85.00, 2, 3),
('Aluminum Plate', 'Marine grade corrosion resistant', 120.00, 1, 1),
('Cement Bag 50kg', 'Premium Portland Cement', 12.50, 3, 2),
('Industrial Router', 'Rugged enterprise networking', 310.00, 2, 3);

INSERT INTO Customer (Name, Email, Phone, Address, City, Country) VALUES
('Ahmed Mohamed', 'ahmed@silsila.com', '+20 100 1234567', '99 Nile Corniche', 'Cairo', 'Egypt'),
('Sara Ali', 'sara@silsila.com', '+20 111 7654321', '45 Port Road', 'Alexandria', 'Egypt'),
('John Doe', 'john.doe@global.com', '+1 555 0001', '10 Main Ave', 'New York', 'USA');

INSERT INTO Warehouse (Name, Location, Capacity) VALUES
('Main Cairo Hub', 'New Cairo, Sector 1', 10000),
('Alex Port Depot', 'Alexandria Free Zone', 5000),
('Ismailia Logistics Center', 'Suez Canal Zone', 15000);

INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available, Reorder_Level) VALUES
(1, 1, 50, 10), (2, 2, 120, 20), (3, 1, 400, 50), (4, 3, 1000, 100), (5, 2, 25, 5);

INSERT INTO Purchase_Order (CustomerID, OrderDate, TotalAmount, Status) VALUES
(1, '2023-12-01', 900.00, 'Completed'),
(2, '2023-12-15', 310.00, 'Processing'),
(3, GETDATE(), 570.00, 'Pending');

INSERT INTO Order_Details (OrderID, ProductID, Quantity, UnitPrice) VALUES
(1, 1, 2, 450.00),
(2, 5, 1, 310.00),
(3, 2, 2, 85.00), (3, 1, 1, 400.00);

INSERT INTO Shipment (OrderID, WarehouseID, Status, Shipped_Via) VALUES
(1, 1, 'Delivered', 'DHL Express'),
(2, 2, 'In Transit', 'Local Logistics');

INSERT INTO Payment (OrderID, Amount, PaymentMethod, Status) VALUES
(1, 900.00, 'Credit Card', 'Completed'),
(3, 570.00, 'Bank Transfer', 'Pending');

INSERT INTO Feedback (CustomerID, Rating, Comment) VALUES
(1, 5, 'Exceptional quality and fast delivery!'),
(2, 4, 'Good communication, but in-transit tracking could be better.');

INSERT INTO Notification (UserID, Message, IsRead) VALUES
(1, 'Monthly inventory report is ready.', 0),
(2, 'Stock level low for Product ID 5.', 1);

