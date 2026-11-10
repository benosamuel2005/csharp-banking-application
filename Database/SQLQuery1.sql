CREATE DATABASE BankingDB;
GO
USE BankingDB;--Uses BankingDB

--Users Table Creation (Admin's details)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
--Customer Table Creation
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    AddressLine1 NVARCHAR(100),
    AddressLine2 NVARCHAR(100),
    City NVARCHAR(50),
    County NVARCHAR(50)
);
--Accounts Table Creation
CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
    AccountNumber BIGINT UNIQUE, -- only 8 digits
    SortCode INT DEFAULT 101010,
    AccountType NVARCHAR(20),  -- Savings/Current
    Balance DECIMAL(18,2),
    OverdraftLimit DECIMAL(18,2) DEFAULT 0
);
--Transaction Table Creation
CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    AccountId INT FOREIGN KEY REFERENCES Accounts(AccountId),
    TransactionType NVARCHAR(20), -- Deposit, Withdraw, Transfer
    Amount DECIMAL(18,2),
    ReferenceNumber NVARCHAR(50),
    TransactionDate DATETIME DEFAULT GETDATE()
);

-- This details is demo purpose
INSERT INTO Users (Username, Password)
VALUES ('admin', '1234');

--Queries to get all data
SELECT * FROM Users; -- Get all Users details
SELECT * FROM Customers; -- Get all details of Customers
SELECT * FROM Accounts; -- Get details Accounts
SELECT * FROM Transactions
ORDER BY TransactionDate DESC; -- Get all the transactions with Timestamp
