SELECT * FROM Product;
SELECT * FROM Customer;
SELECT * FROM Purchase_Order;
SELECT * FROM Supplier;

SELECT Product_Name, Price FROM Product WHERE CategoryID = 1;
SELECT * FROM Purchase_Order WHERE CustomerID = 1;
SELECT ProductID, Quantity_Available FROM Inventory WHERE Quantity_Available <= Reorder_Level;

SELECT o.OrderID, p.Product_Name, d.Quantity, d.UnitPrice
FROM Order_Details d
JOIN Product p ON d.ProductID = p.ProductID
JOIN Purchase_Order o ON d.OrderID = o.OrderID;

SELECT c.CustomerID, c.Name, u.Email
FROM Customer c
JOIN [User] u ON c.UserID = u.UserID;

SELECT s.OrderID, w.Name AS Warehouse, s.Status
FROM Shipment s
JOIN Warehouse w ON s.WarehouseID = w.WarehouseID;

SELECT c.Name, SUM(o.TotalAmount) AS Total_Spent
FROM Purchase_Order o
JOIN Customer c ON o.CustomerID = c.CustomerID
GROUP BY c.Name;

SELECT p.Product_Name, SUM(d.Quantity) AS Total_Sold
FROM Order_Details d
JOIN Product p ON d.ProductID = p.ProductID
GROUP BY p.Product_Name
ORDER BY Total_Sold DESC;

UPDATE Purchase_Order SET Status = 'Delivered' WHERE OrderID = 4;
UPDATE Inventory SET Quantity_Available = Quantity_Available - 1 WHERE ProductID = 1 AND WarehouseID = 1;

DELETE FROM Notification WHERE UserID = 2;
DELETE FROM Order_Details WHERE OrderID = 2;

