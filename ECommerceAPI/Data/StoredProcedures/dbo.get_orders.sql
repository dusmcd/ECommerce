CREATE PROCEDURE get_orders
AS
BEGIN
	SELECT ord.Id as OrderId, ord.OrderDate, ord.ShippedDate, ord.FulfilledDate,
	pro.Id as ProductId, pro.Name as ProductName, pro.Price, pro.Description,
	cust.Id as CustomerId, cust.Name as CustomerName, cust.Email, po.Quantity
	FROM Orders ord
	INNER JOIN Customers cust ON ord.CustomerId = cust.Id
	INNER JOIN ProductsOrders po ON ord.Id = po.OrderId
	INNER JOIN Products pro ON po.ProductId = pro.Id
	ORDER BY ord.Id;
END;