/* =========================================================
   CSAI202 - PHASE 1: SQL QUERIES & TEST DATA
   Team Member: Mahmoud Sakr
   ========================================================= */

-- TEST DATA INSERTION (Satisfies "Random Data" Requirement)
-- We will use these to populate the database in Phase 2.

-- Insert Categories
INSERT INTO Category (Name, Description) VALUES 
('Electronics', 'Devices and Gadgets'),
('Furniture', 'Office and Home Furniture');

-- Insert Products
INSERT INTO Product (Product_Name, Description, Price, CategoryID) VALUES 
('Dell XPS 15', 'High-performance laptop', 45000.00, 1),
('Logitech Mouse', 'Wireless Optical Mouse', 650.00, 1),
('Office Chair', 'Ergonomic Black Chair', 3500.00, 2);

-- Insert Users (Roles: 1=Admin, 2=Customer)
INSERT INTO [User] (User_name, Email, Password, RoleID) VALUES 
('AdminUser', 'admin@scm.com', 'securePass123', 1),
('TestCustomer', 'client@scm.com', 'clientPass123', 2);

--  APPLICATION QUERIES 

--  Create a New Order (For Checkout Page)
INSERT INTO Purchase_Order (CustomerID, OrderDate, Status, TotalAmount)
VALUES (@CustomerID, GETDATE(), 'Pending', @TotalAmount);

--  Get Order History (For My Orders Page)
SELECT OrderID, OrderDate, Status, TotalAmount 
FROM Purchase_Order 
WHERE CustomerID = @CustomerID;

-- Update Inventory (After Order Placed)
UPDATE Inventory 
SET Quantity_Available = Quantity_Available - @OrderedQty
WHERE Product_ID = @ProductID;