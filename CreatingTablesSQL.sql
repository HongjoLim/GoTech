CREATE TABLE Customer(
	[Id] INT IDENTITY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[Gender] CHAR(1) NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[Province] VARCHAR(30) NOT NULL,
	[PostalCode] VARCHAR(7) NOT NULL,
	[Email] VARCHAR(60) NOT NULL,
	[PhoneNumber] VARCHAR(13) NOT NULL
);

CREATE TABLE Employee(
	[Id] INT IDENTITY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[Gender] CHAR(1) NOT NULL,
	[DepartmentId] INT NOT NULL,
	[PositionId] INT NOT NULL, 
	[ProjectId] INT NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[Province] VARCHAR(30) NOT NULL,
	[PostalCode] VARCHAR(7) NOT NULL,
	[Email] VARCHAR(60) NOT NULL,
	[PhoneNumber] VARCHAR(13) NOT NULL
);

CREATE TABLE PositionTitle(
	[Id] INT IDENTITY,
	[Name] VARCHAR(30) NOT NULL
);

CREATE TABLE Branch(
	[Id] INT IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[BranchManagerId] INT NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[Province] VARCHAR(30) NOT NULL,
	[PostalCode] VARCHAR(7) NOT NULL,
	[Email] VARCHAR(60) NOT NULL,
	[PhoneNumber] VARCHAR(13) NOT NULL
);

CREATE TABLE Department(
	[Id] INT IDENTITY,
	[BranchId] INT NOT NULL,
	[Name] VARCHAR(30) NOT NULL
);

CREATE TABLE Proejct(
	[Id] INT IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	[DeadLine] DATETIME NOT NULL,
	[Description] VARCHAR(MAX) NOT NULL,
);