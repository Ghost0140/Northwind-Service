USE Northwind
GO

SET NOCOUNT ON
GO

/* =================== **Solucionando defectos de las tablas Customers y Suppliers** ================== */

CREATE TABLE Paises (
    PaisID INT IDENTITY(1,1) PRIMARY KEY,
    NombrePais NVARCHAR(50) NOT NULL UNIQUE
);
GO

INSERT INTO Paises (NombrePais) 
VALUES 
    ('USA'), ('UK'), ('Germany'), ('France'), ('Canada'), ('Austria'),
    ('Belgium'), ('Denmark'), ('Finland'), ('Italy'), ('Spain'), ('Sweden'),
    ('Switzerland'), ('Brazil'), ('Argentina'), ('Portugal'), ('Venezuela'),
    ('Mexico'), ('Norway'), ('Poland'), ('Ireland');
GO

CREATE TABLE Cargos (
    CargoID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCargo NVARCHAR(50) NOT NULL UNIQUE
);
GO

INSERT INTO Cargos (NombreCargo) VALUES 
('Sales Representative'),
('Owner'),
('Marketing Manager'),
('Sales Manager'),
('Accounting Manager'),
('Sales Associate'),
('Marketing Assistant'),
('Sales Agent'),
('Assistant Sales Agent'),
('Assistant Sales Representative'),
('Order Administrator'),
('Owner/Marketing Assistant');
GO

INSERT INTO Cargos (NombreCargo)
SELECT DISTINCT ContactTitle 
FROM Suppliers
WHERE ContactTitle NOT IN (SELECT NombreCargo FROM Cargos);
GO

ALTER TABLE Customers ADD CargoID INT;
ALTER TABLE Customers ADD PaisID INT;
GO

UPDATE C
SET C.CargoID = CA.CargoID
FROM Customers C
INNER JOIN Cargos CA ON C.ContactTitle = CA.NombreCargo;
GO

UPDATE C
SET C.PaisID = P.PaisID
FROM Customers C
INNER JOIN Paises P ON C.Country = P.NombrePais;
Go

ALTER TABLE Customers 
ADD CONSTRAINT FK_Customers_Cargos FOREIGN KEY (CargoID) REFERENCES Cargos(CargoID);
GO

ALTER TABLE Customers 
ADD CONSTRAINT FK_Customers_Paises FOREIGN KEY (PaisID) REFERENCES Paises(PaisID);
GO

ALTER TABLE Customers DROP COLUMN ContactTitle;
ALTER TABLE Customers DROP COLUMN Country;
GO

ALTER TABLE Suppliers ADD CargoID INT;
ALTER TABLE Suppliers ADD PaisID INT;
GO

UPDATE S
SET S.CargoID = C.CargoID
FROM Suppliers S
INNER JOIN Cargos C ON S.ContactTitle = C.NombreCargo;
GO

UPDATE S
SET S.PaisID = P.PaisID
FROM Suppliers S
INNER JOIN Paises P ON S.Country = P.NombrePais;
GO

ALTER TABLE Suppliers 
ADD CONSTRAINT FK_Suppliers_Cargos FOREIGN KEY (CargoID) REFERENCES Cargos(CargoID);
GO

ALTER TABLE Suppliers 
ADD CONSTRAINT FK_Suppliers_Paises FOREIGN KEY (PaisID) REFERENCES Paises(PaisID);
GO

ALTER TABLE Suppliers DROP COLUMN ContactTitle;
ALTER TABLE Suppliers DROP COLUMN Country;
GO

UPDATE Suppliers 
SET PaisID = 1 
WHERE PaisID IS NULL;
GO

/* =================== Paises ================== */

 -- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Paises_ListarTodo
AS
BEGIN
    SELECT 
        PaisID, 
        NombrePais
    FROM 
        Paises;
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Paises_ListarPorId
    @PaisID INT
AS
BEGIN
    SELECT 
        PaisID, 
        NombrePais
    FROM 
        Paises
    WHERE 
        PaisID = @PaisID;
END
GO

/* =================== Cargos ================== */

 -- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Cargos_ListarTodo
AS
BEGIN
    SELECT 
        CargoID, 
        NombreCargo
    FROM 
        Cargos;
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Cargos_ListarPorId
    @CargoID INT
AS
BEGIN
    SELECT 
        CargoID, 
        NombreCargo
    FROM 
        Cargos
    WHERE 
        CargoID = @CargoID;
END
GO

/* =================== Productos ================== */

-- 1. Listar Todos (SOLO ACTIVOS - Regla de Negocio)
CREATE OR ALTER PROCEDURE USP_Productos_ListarTodo
    @NombreProducto NVARCHAR(40) = NULL
AS
BEGIN
    SELECT  p.ProductID,
            p.ProductName,
            p.SupplierID,
            s.CompanyName AS SupplierName,
            p.CategoryID,
            c.CategoryName,
            p.QuantityPerUnit,
            p.UnitPrice,
            p.UnitsInStock,
            p.UnitsOnOrder,
            p.ReorderLevel
    FROM Products p
    INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID
    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
    WHERE p.Discontinued = 0
      AND (
            @NombreProducto IS NULL
            OR @NombreProducto = ''
            OR p.ProductName LIKE '%' + @NombreProducto + '%'
          );
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Productos_ListarPorId
    @ProductID INT
AS
BEGIN
    SELECT  p.ProductID,
            p.ProductName,
            p.SupplierID,
            s.CompanyName AS SupplierName,
            p.CategoryID,
            c.CategoryName,
            p.QuantityPerUnit,
            p.UnitPrice,
            p.UnitsInStock,
            p.UnitsOnOrder,
            p.ReorderLevel
    FROM Products p
    INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID
    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
    WHERE p.ProductID = @ProductID AND Discontinued = 0;
END
GO

-- 3. Insertar (NO INCLUYE Discontinued - Regla de Negocio)
CREATE OR ALTER PROCEDURE USP_Productos_Insertar
    @ProductName NVARCHAR(40),
    @SupplierID INT,
    @CategoryID INT,
    @QuantityPerUnit NVARCHAR(20),
    @UnitPrice MONEY,
    @UnitsInStock SMALLINT,
    @UnitsOnOrder SMALLINT,
    @ReorderLevel SMALLINT
AS
BEGIN
    INSERT INTO Products (
        ProductName, 
        SupplierID, 
        CategoryID, 
        QuantityPerUnit, 
        UnitPrice, 
        UnitsInStock, 
        UnitsOnOrder, 
        ReorderLevel, 
        Discontinued -- Se omite en el parámetro, se inserta 0 (por defecto)
    )
    VALUES (
        @ProductName, 
        @SupplierID, 
        @CategoryID, 
        @QuantityPerUnit, 
        @UnitPrice, 
        @UnitsInStock, 
        @UnitsOnOrder, 
        @ReorderLevel,
        0 -- Valor por defecto: Activo
    )
END
GO

-- 4. Actualizar (NO INCLUYE Discontinued - Regla de Negocio)
CREATE OR ALTER PROCEDURE USP_Productos_Actualizar
    @ProductID INT,
    @ProductName NVARCHAR(40),
    @SupplierID INT,
    @CategoryID INT,
    @QuantityPerUnit NVARCHAR(20),
    @UnitPrice MONEY,
    @UnitsInStock SMALLINT,
    @UnitsOnOrder SMALLINT,
    @ReorderLevel SMALLINT
AS
BEGIN
    UPDATE Products
    SET 
        ProductName = @ProductName,
        SupplierID = @SupplierID,
        CategoryID = @CategoryID,
        QuantityPerUnit = @QuantityPerUnit,
        UnitPrice = @UnitPrice,
        UnitsInStock = @UnitsInStock,
        UnitsOnOrder = @UnitsOnOrder,
        ReorderLevel = @ReorderLevel
        -- Discontinued no se actualiza aquí
    WHERE 
        ProductID = @ProductID;
END
GO

-- 5. Eliminación (SOFT DELETE - Cambia Discontinued de 0 a 1 - Regla de Negocio)
CREATE OR ALTER PROCEDURE USP_Productos_Eliminar
    @ProductID INT
AS
BEGIN
    UPDATE Products
    SET 
        Discontinued = 1
    WHERE 
        ProductID = @ProductID
END
GO

/*
EXEC USP_Productos_ListarTodo
GO

EXEC USP_Productos_ListarPorId 1
GO

EXEC USP_Productos_Insertar
      @ProductName = 'Café Premium',
      @SupplierID = 4,
      @CategoryID = 1,
      @QuantityPerUnit = '500 g',
      @UnitPrice = 12.50,
      @UnitsInStock = 20,
      @UnitsOnOrder = 5,
      @ReorderLevel = 10
GO

EXEC USP_Productos_Actualizar
      @ProductID = 78,
      @ProductName = 'Café Premium',
      @SupplierID = 5,
      @CategoryID = 1,
      @QuantityPerUnit = '1 kg',
      @UnitPrice = 12.50,
      @UnitsInStock = 20,
      @UnitsOnOrder = 5,
      @ReorderLevel = 10
GO

EXEC USP_Productos_Eliminar 78
GO
*/

/* =================== Categorías ================== */

 -- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Categorias_ListarTodo
AS
BEGIN
    SELECT 
        CategoryID, 
        CategoryName, 
        Description
    FROM 
        Categories;
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Categorias_ListarPorId
    @CategoryID INT
AS
BEGIN
    SELECT 
        CategoryID, 
        CategoryName, 
        Description
    FROM 
        Categories
    WHERE 
        CategoryID = @CategoryID;
END
GO

-- 3. Insertar
CREATE OR ALTER PROCEDURE USP_Categorias_Insertar
    @CategoryName NVARCHAR(15),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Categories (CategoryName, Description)
    VALUES (@CategoryName, @Description);
END
GO

-- 4. Actualizar
CREATE OR ALTER PROCEDURE USP_Categorias_Actualizar
    @CategoryID INT,
    @CategoryName NVARCHAR(15),
    @Description NVARCHAR(MAX)
AS
BEGIN
    UPDATE Categories
    SET 
        CategoryName = @CategoryName,
        Description = @Description
    WHERE 
        CategoryID = @CategoryID;
END
GO

/*
EXEC USP_Categorias_ListarTodo
GO

EXEC USP_Categorias_ListarPorId 1
GO

EXEC USP_Categorias_Insertar
    @CategoryName = N'Bebidas Gourmet',
    @Description = N'Productos de café, té y bebidas artesanales.'
GO

EXEC USP_Categorias_Actualizar
    @CategoryID = 9,
    @CategoryName = N'Bebidas G.',
    @Description = N'Productos de café, té y bebidas artesanales.'
GO
*/

/* =================== Clientes ================== */

-- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Clientes_ListarTodo
    @CompanyName NVARCHAR(40) = NULL
AS
BEGIN
    SELECT c.CustomerID, c.CompanyName, c.ContactName, c.CargoID, cg.NombreCargo, 
           c.Address, c.PostalCode, c.PaisID, p.NombrePais, c.Phone
    FROM Customers c
    INNER JOIN Cargos cg ON c.CargoID = cg.CargoID
    INNER JOIN Paises p ON c.PaisID = p.PaisID
    WHERE
        (@CompanyName IS NULL OR @CompanyName = '')
        OR CompanyName LIKE '%' + @CompanyName + '%'
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Clientes_ListarPorId
    @CustomerID NCHAR(5)
AS
BEGIN
    SELECT c.CustomerID, c.CompanyName, c.ContactName, c.CargoID, cg.NombreCargo, 
           c.Address, c.PostalCode, c.PaisID, p.NombrePais, c.Phone
    FROM Customers c
    INNER JOIN Cargos cg ON c.CargoID = cg.CargoID
    INNER JOIN Paises p ON c.PaisID = p.PaisID
    WHERE 
        CustomerID = @CustomerID;
END
GO

-- 3. Insertar
CREATE OR ALTER PROCEDURE USP_Clientes_Insertar
    @CustomerID NCHAR(5),
    @CompanyName NVARCHAR(40),
    @ContactName NVARCHAR(30),
    @CargoID INT,
    @Address NVARCHAR(60),
    @PostalCode NVARCHAR(10),
    @PaisID INT,
    @Phone NVARCHAR(24)
AS
BEGIN
    INSERT INTO Customers (
        CustomerID,
        CompanyName,
        ContactName,
        CargoID,
        Address, 
        PostalCode,
        PaisID, 
        Phone
    )
    VALUES (
        UPPER(@CustomerID),
        @CompanyName, 
        @ContactName,
        @CargoID,
        @Address,
        @PostalCode,
        @PaisID, 
        @Phone
    );
END
GO

-- 4. Actualizar
CREATE OR ALTER PROCEDURE USP_Clientes_Actualizar
    @CustomerID NCHAR(5),
    @CompanyName NVARCHAR(40),
    @ContactName NVARCHAR(30),
    @CargoID INT,
    @Address NVARCHAR(60),
    @PostalCode NVARCHAR(10),
    @PaisID INT,
    @Phone NVARCHAR(24)
AS
BEGIN
    UPDATE Customers
    SET CompanyName = @CompanyName,
        ContactName = @ContactName,
        CargoID = @CargoID,
        Address = @Address,
        PostalCode = @PostalCode,
        PaisID = @PaisID,
        Phone = @Phone
    WHERE CustomerID = @CustomerID;
END
GO

/*
EXEC USP_Clientes_ListarTodo
GO

EXEC USP_Clientes_ListarPorId 'ALFKI'
GO

EXEC USP_Clientes_Insertar
    @CustomerID = N'AAAAA',
    @CompanyName = N'Comercial Los Andes',
    @ContactName = N'Juan Pérez',
    @CargoID = 1,
    @Address = N'Av. Central 123',
    @PostalCode = N'15001',
    @PaisID = 1,
    @Phone = N'+51 987 654 321'
GO

EXEC USP_Clientes_Actualizar
    @CustomerID = N'AAAAA',
    @CompanyName = N'Comercial Cor. Andes',
    @ContactName = N'Juan Pérez',
    @CargoID = 1,
    @Address = N'Av. Central 123',
    @PostalCode = N'15001',
    @PaisID = 2,
    @Phone = N'+51 987 654 321'
GO
*/

/* =================== Empleados ================== */

-- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Empleados_ListarTodo
AS
BEGIN
    SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy,
           BirthDate, HireDate, Address, City, Region, PostalCode,
           Country, HomePhone
    FROM Employees;
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Empleados_ListarPorId
    @EmployeeID INT
AS
BEGIN
    SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy,
           BirthDate, HireDate, Address, City, Region, PostalCode,
           Country, HomePhone
    FROM Employees 
    WHERE EmployeeID = @EmployeeID;
END
GO

-- 3. Insertar
CREATE OR ALTER PROCEDURE USP_Empleados_Insertar
    @LastName NVARCHAR(20),
    @FirstName NVARCHAR(10),
    @Title NVARCHAR(30),
    @TitleOfCourtesy NVARCHAR(25),
    @BirthDate DATETIME,
    @HireDate DATETIME,
    @Address NVARCHAR(60),
    @City NVARCHAR(15),
    @Region NVARCHAR(15),
    @PostalCode NVARCHAR(10),
    @Country NVARCHAR(15),
    @HomePhone NVARCHAR(24)
AS
BEGIN
    INSERT INTO Employees
    (LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate,
     Address, City, Region, PostalCode, Country, HomePhone)
    VALUES
    (@LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate,
     @Address, @City, @Region, @PostalCode, @Country, @HomePhone);
END
GO

-- 4. Actualizar
CREATE OR ALTER PROCEDURE USP_Empleados_Actualizar
    @EmployeeID INT,
    @LastName NVARCHAR(20),
    @FirstName NVARCHAR(10),
    @Title NVARCHAR(30),
    @TitleOfCourtesy NVARCHAR(25),
    @BirthDate DATETIME,
    @HireDate DATETIME,
    @Address NVARCHAR(60),
    @City NVARCHAR(15),
    @Region NVARCHAR(15),
    @PostalCode NVARCHAR(10),
    @Country NVARCHAR(15),
    @HomePhone NVARCHAR(24)
AS
BEGIN
    UPDATE Employees
    SET LastName = @LastName,
        FirstName = @FirstName,
        Title = @Title,
        TitleOfCourtesy = @TitleOfCourtesy,
        BirthDate = @BirthDate,
        HireDate = @HireDate,
        Address = @Address,
        City = @City,
        Region = @Region,
        PostalCode = @PostalCode,
        Country = @Country,
        HomePhone = @HomePhone
    WHERE EmployeeID = @EmployeeID;
END
GO

/*
EXEC USP_Empleados_ListarTodo
GO

EXEC USP_Empleados_ListarPorId 1
GO

EXEC USP_Empleados_Insertar
    @LastName = N'García',
    @FirstName = N'Marcos',
    @Title = N'Analista de Sistemas',
    @TitleOfCourtesy = N'Sr.',
    @BirthDate = '1988-05-12',
    @HireDate = '2023-02-01',
    @Address = N'Av. Libertad 456',
    @City = N'Santiago',
    @Region = N'Metropolitana',
    @PostalCode = N'8320000',
    @Country = N'Chile',
    @HomePhone = N'+56 2 9876 5432'
GO

EXEC USP_Empleados_Actualizar
    @EmployeeID = 10,
    @LastName = N'San',
    @FirstName = N'Marcos',
    @Title = N'Analista de Sistemas',
    @TitleOfCourtesy = N'Sr.',
    @BirthDate = '1988-05-12',
    @HireDate = '2023-02-01',
    @Address = N'Av. Libertad 456',
    @City = N'Santiago',
    @Region = N'Metropolitana',
    @PostalCode = N'8320000',
    @Country = N'Chile',
    @HomePhone = N'+56 2 9876 5432'
GO
*/

/* =================== Proveedores ================== */

-- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Proveedores_ListarTodo
    @CompanyName NVARCHAR(40) = NULL
AS
BEGIN
    SELECT s.SupplierID, s.CompanyName, s.ContactName, s.CargoID, c.NombreCargo,
           s.Address, s.PostalCode, s.PaisID, p.NombrePais, s.Phone
    FROM Suppliers s
    INNER JOIN Cargos c ON s.CargoID = c.CargoID
    INNER JOIN Paises p ON s.PaisID = p.PaisID
    WHERE
        (@CompanyName IS NULL OR @CompanyName = '')
        OR s.CompanyName LIKE '%' + @CompanyName + '%'
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Proveedores_ListarPorId
    @SupplierID INT
AS
BEGIN
    SELECT s.SupplierID, s.CompanyName, s.ContactName, s.CargoID, c.NombreCargo,
           s.Address, s.PostalCode, s.PaisID, p.NombrePais, s.Phone
    FROM Suppliers s
    INNER JOIN Cargos c ON s.CargoID = c.CargoID
    INNER JOIN Paises p ON s.PaisID = p.PaisID
    WHERE SupplierId = @SupplierID
END
GO

-- 3. Insertar
CREATE OR ALTER PROCEDURE USP_Proveedores_Insertar
    @CompanyName NVARCHAR(40),
    @ContactName NVARCHAR(30),
    @CargoID INT,
    @Address NVARCHAR(60),
    @PostalCode NVARCHAR(10),
    @PaisID INT,
    @Phone NVARCHAR(24)
AS
BEGIN
    INSERT INTO Suppliers
    (CompanyName, ContactName, CargoID, Address,
     PostalCode, PaisID, Phone)
    VALUES
    (@CompanyName, @ContactName, @CargoID, @Address,
     @PostalCode, @PaisID, @Phone);
END
GO

-- 4. Actualizar
CREATE OR ALTER PROCEDURE USP_Proveedores_Actualizar
    @SupplierID INT,
    @CompanyName NVARCHAR(40),
    @ContactName NVARCHAR(30),
    @CargoID INT,
    @Address NVARCHAR(60),
    @PostalCode NVARCHAR(10),
    @PaisID INT,
    @Phone NVARCHAR(24)
AS
BEGIN
    UPDATE Suppliers
    SET CompanyName = @CompanyName,
        ContactName = @ContactName,
        CargoID = @CargoID,
        Address = @Address,
        PostalCode = @PostalCode,
        PaisID = @PaisID,
        Phone = @Phone
    WHERE SupplierID = @SupplierID;
END
GO

/*
EXEC USP_Proveedores_ListarTodo
GO

EXEC USP_Proveedores_ListarPorId 1
GO

EXEC USP_Proveedores_Insertar
    @CompanyName = N'Importadora Global',
    @ContactName = N'Laura Rivas',
    @CargoID = 1,
    @Address = N'Av. Industrial 789',
    @PostalCode = N'110111',
    @PaisID = 1,
    @Phone = N'+57 1 234 5678'
GO

EXEC USP_Proveedores_Actualizar
    @SupplierID = 30,
    @CompanyName = N'Importadora Global NN',
    @ContactName = N'Laura Rivas',
    @CargoID = 2,
    @Address = N'Av. Industrial 789',
    @PostalCode = N'110111',
    @PaisID = 1,
    @Phone = N'+57 1 234 5678'
GO
*/

/* =================== Transportistas ================== */

-- 1. Listar Todos
CREATE OR ALTER PROCEDURE USP_Transportistas_ListarTodo
    @CompanyName NVARCHAR(40) = NULL
AS
BEGIN
    SELECT ShipperID, CompanyName, Phone
    FROM Shippers
    WHERE
        (@CompanyName IS NULL OR @CompanyName = '')
        OR CompanyName LIKE '%' + @CompanyName + '%'
END
GO

-- 2. Listar por ID
CREATE OR ALTER PROCEDURE USP_Transportistas_ListarPorId
    @ShipperID INT
AS
BEGIN
    SELECT ShipperID, CompanyName, Phone
    FROM Shippers
    WHERE ShipperID = @ShipperID
END
GO

-- 3. Insertar
CREATE OR ALTER PROCEDURE USP_Transportistas_Insertar
    @CompanyName NVARCHAR(40),
    @Phone NVARCHAR(24)
AS
BEGIN
    INSERT INTO Shippers (CompanyName, Phone)
    VALUES (@CompanyName, @Phone);
END
GO

-- 4. Actualizar
CREATE OR ALTER PROCEDURE USP_Transportistas_Actualizar
    @ShipperID INT,
    @CompanyName NVARCHAR(40),
    @Phone NVARCHAR(24)
AS
BEGIN
    UPDATE Shippers
    SET CompanyName = @CompanyName,
        Phone = @Phone
    WHERE ShipperID = @ShipperID;
END
GO

/*
EXEC USP_Transportistas_ListarTodo
GO

EXEC USP_Transportistas_ListarPorId 1
GO

EXEC USP_Transportistas_Insertar
    @CompanyName = N'Logística Express',
    @Phone = N'+52 55 6789 4321'
GO

EXEC USP_Transportistas_Actualizar
    @ShipperID = 4,
    @CompanyName = N'Logística Express',
    @Phone = N'+52 55 6789 1111'
GO
*/

/* =================== Consultas ================== */

CREATE OR ALTER PROCEDURE USP_Orders_ListarEntreFechas
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        OrderID, 
        CustomerID,     
        EmployeeID,  
        OrderDate,
        ShipName,
        ShipCountry
    FROM Orders
    WHERE OrderDate BETWEEN @FechaInicio AND @FechaFin
    ORDER BY OrderDate;
END
GO

CREATE OR ALTER PROCEDURE USP_Pedidos_Por_Cliente
    @CustomerID NVARCHAR(5) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderID,
        o.CustomerID,
        c.CompanyName,
        o.OrderDate,
        o.RequiredDate,
        o.ShippedDate,
        o.ShipName,
        o.ShipCity,
        o.ShipCountry
    FROM Orders o
    INNER JOIN Customers c
        ON o.CustomerID = c.CustomerID
    WHERE 
        (@CustomerID IS NULL OR @CustomerID = '')
        OR o.CustomerID = @CustomerID
    ORDER BY o.OrderDate DESC;
END;
GO

/* =================== Reportes ================== */

-- 1. Caso de Reporte (Top 10 Productos con más Stock)
CREATE OR ALTER PROCEDURE USP_Reporte_Top10ProductosEnStock
AS
BEGIN
    SELECT TOP 10
        p.ProductName,
        p.UnitsInStock,
        c.CategoryName
    FROM 
        Products p
    INNER JOIN 
        Categories c ON p.CategoryID = c.CategoryID
    WHERE 
        p.Discontinued = 0
    ORDER BY 
        p.UnitsInStock DESC;
END
GO

-- 2. Caso de Reporte (Total de Pedidos por Empleado)
CREATE OR ALTER PROCEDURE USP_Reporte_TotalOrdenesPorEmpleado
AS
BEGIN
    SELECT 
        E.EmployeeID,
        E.LastName,
        E.FirstName,
        COUNT(O.OrderID) AS TotalOrders
    FROM 
        Employees E
    LEFT JOIN 
        Orders O ON E.EmployeeID = O.EmployeeID
    GROUP BY 
        E.EmployeeID, E.LastName, E.FirstName
    ORDER BY 
        TotalOrders DESC;
END
GO

-- 3. Caso de Reporte (Total de Productos por Proveedor)
CREATE OR ALTER PROCEDURE USP_Reporte_TotalProductosPorProveedor
AS
BEGIN
    SELECT 
        S.SupplierID,
        S.CompanyName,
        S.ContactName,
        COUNT(P.ProductID) AS TotalProducts
    FROM 
        Suppliers S
    LEFT JOIN 
        Products P ON S.SupplierID = P.SupplierID
    GROUP BY 
        S.SupplierID, S.CompanyName, S.ContactName
    ORDER BY 
        TotalProducts DESC;
END
GO



CREATE OR ALTER PROCEDURE USP_Orders_ListarEntreFechas
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderID,               -- 0
        o.CustomerID,            -- 1
        c.CompanyName,           -- 2
        o.OrderDate,             -- 3
        o.RequiredDate,          -- 4
        o.ShippedDate,           -- 5
        o.ShipName,              -- 6
        o.ShipCity,              -- 7
        o.ShipCountry             -- 8
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerID = c.CustomerID
    WHERE o.OrderDate BETWEEN @FechaInicio AND @FechaFin
    ORDER BY o.OrderDate;
END;
GO

/*
EXEC USP_Orders_ListarEntreFechas '1996-01-01', '1996-12-31'
go
*/

/* =================== Adicional ================== */

-- Totales
CREATE OR ALTER PROCEDURE USP_Adicional_ObtenerTotales
AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Products WHERE Discontinued = 0)     AS TotalProductos,
        (SELECT COUNT(*) FROM Customers)    AS TotalClientes,
        (SELECT COUNT(*) FROM Shippers)     AS TotalTransportistas,
        (SELECT COUNT(*) FROM Suppliers)    AS TotalProveedores
END
GO
