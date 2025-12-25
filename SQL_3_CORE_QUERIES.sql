
-- Basic Queries

SELECT P.Product_Name, I.Quantity_Available, W.Name AS Warehouse
FROM Inventory I
JOIN Product P ON I.ProductID = P.ProductID
JOIN Warehouse W ON I.WarehouseID = W.WarehouseID;

SELECT Name, TotalAmount, Status
FROM Purchase_Order PO
JOIN Customer C ON PO.CustomerID = C.CustomerID;

SELECT Name, Capacity
FROM Warehouse;

