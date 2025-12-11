
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MyDatabase')
BEGIN
  CREATE DATABASE MyDatabase;
END
GO

USE MyDatabase;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TableName' and xtype='U')
BEGIN
  CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    OrderTotal DECIMAL(18, 2) NOT NULL
  );
END
GO
