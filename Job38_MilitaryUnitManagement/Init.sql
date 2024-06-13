CREATE DATABASE MilitaryUnitManagement;
GO

USE MilitaryUnitManagement;
GO

CREATE TABLE Battalion (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NTEXT
);
GO

CREATE TABLE Company (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    FK_BattalionID INT,
    Description NTEXT,
    FOREIGN KEY (FK_BattalionID) REFERENCES Battalion(ID)
);
GO

CREATE TABLE Platoon (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    FK_CompanyID INT,
    Description NTEXT,
    FOREIGN KEY (FK_CompanyID) REFERENCES Company(ID)
);
GO




INSERT INTO Battalion (Name, Description) VALUES
(N'Tiểu đoàn 1', N'Mô tả Tiểu đoàn 1'),
(N'Tiểu đoàn 2', N'Mô tả Tiểu đoàn 2'),
(N'Tiểu đoàn 3', N'Mô tả Tiểu đoàn 3');
GO

INSERT INTO Company (Name, FK_BattalionID, Description) VALUES
(N'Đại đội A', 1, N'Đại đội đầu tiên của Tiểu đoàn 1'),
(N'Đại đội B', 1, N'Đại đội thứ hai của Tiểu đoàn 1'),
(N'Đại đội C', 2, N'Đại đội đầu tiên của Tiểu đoàn 2'),
(N'Đại đội D', 3, N'Đại đội đầu tiên của Tiểu đoàn 3');
GO

INSERT INTO Platoon (Name, FK_CompanyID, Description) VALUES
(N'Trung đội Alpha', 1, N'Trung đội đầu tiên của Đại đội A'),
(N'Trung đội Beta', 1, N'Trung đội thứ hai của Đại đội A'),
(N'Trung đội Gamma', 2, N'Trung đội đầu tiên của Đại đội B'),
(N'Trung đội Delta', 3, N'Trung đội đầu tiên của Đại đội C'),
(N'Trung đội Epsilon', 4, N'Trung đội đầu tiên của Đại đội D');
GO



SELECT * FROM Platoon INNER JOIN Company ON Platoon.FK_CompanyID = Company.ID
WHERE 
SELECT * FROM Company
SELECT * FROM Battalion




DROP TABLE Platoon
DROP TABLE Company
DROP TABLE Battalion