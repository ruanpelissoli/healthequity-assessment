SELECT 
	CONCAT(C.FirstName, ' ', C.LastName) as 'FullName',
	C.Age,
	O.OrderID,
	O.DateCreated,
	O.MethodOfPurchase as 'PurchaseMethod',
	OD.ProductNumber,
	OD.ProductOrigin
FROM Customer C
INNER JOIN Orders O ON C.PersonID = O.PersonId
INNER JOIN OrdersDetails OD ON O.OrderID = OD.OrderId
WHERE OD.ProductID = 1112222333