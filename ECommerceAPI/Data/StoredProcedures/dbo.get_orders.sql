CREATE PROCEDURE get_orders
    @limit INT = -1
AS
BEGIN
    IF @limit > 0
    BEGIN
        -- Use dynamic SQL for the TOP clause
        DECLARE @sql NVARCHAR(MAX);
        SET @sql = 'SELECT TOP ' + CAST(@limit AS NVARCHAR) + 
					'ord.Id as OrderId, ord.OrderDate, ord.ShippedDate, ord.FulfilledDate,'+
					'pro.Id as ProductId, pro.Name as ProductName, pro.Price, pro.Description,'+
					'cust.Id as CustomerId, cust.Name as CustomerName,cust.Email, po.Quantity'+
                   'INNER JOIN Customers cust ON ord.CustomerId = cust.Id ' +
                   'INNER JOIN ProductsOrders po ON ord.Id = po.OrderId ' +
                   'INNER JOIN Products pro ON po.ProductId = pro.Id ORDER BY ord.Id';
        EXEC sp_executesql @sql;
    END
    ELSE
    BEGIN
        -- Retrieve all records if @limit is not greater than 0
        SELECT ord.Id as OrderId, ord.OrderDate, ord.ShippedDate, ord.FulfilledDate,
		pro.Id as ProductId, pro.Name as ProductName, pro.Price, pro.Description,
		cust.Id as CustomerId, cust.Name as CustomerName, cust.Email, po.Quantity
        FROM Orders ord
        INNER JOIN Customers cust ON ord.CustomerId = cust.Id
        INNER JOIN ProductsOrders po ON ord.Id = po.OrderId
        INNER JOIN Products pro ON po.ProductId = pro.Id
		ORDER BY ord.Id;
    END
END;