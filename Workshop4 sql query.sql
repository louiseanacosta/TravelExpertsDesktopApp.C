select * from Packages
select * from Packages_Products_Suppliers
select * from Products_Suppliers
select * from Suppliers
select * from Products

--- SELECT PRODUCTS THAT ARE INCLUDED IN PACKAGES
SELECT q.ProductSupplierId, p.ProdName, s.SupName
FROM Products p, Products_Suppliers q, Suppliers s, Packages t, Packages_Products_Suppliers u
WHERE p.ProductId = q.ProductId and s.SupplierId = q.SupplierId and q.ProductSupplierId = u.ProductSupplierId and t.PackageId=u.PackageId and t.PackageId=2



--- SELECT PRODUCTS THAT ARE NOT INCLUDED IN PACKAGES
SELECT q.ProductSupplierId, p.ProdName, s.SupName
FROM Products p, Products_Suppliers q, Suppliers s
WHERE p.ProductId = q.ProductId and s.SupplierId = q.SupplierId and ProductSupplierId NOT IN
(SELECT ProductSupplierId FROM Packages_Products_Suppliers)
ORDER BY ProductSupplierId


-- SELECT ALL PRODUCTS (BOTH INCLUDED AND NOT INCLUDED IN PACKAGES
SELECT ps.ProductSupplierId, p.ProdName, s.SupName
FROM Products p, Products_Suppliers ps, Suppliers s
WHERE p.ProductId = ps.ProductId and s.SupplierId = ps.SupplierId 

