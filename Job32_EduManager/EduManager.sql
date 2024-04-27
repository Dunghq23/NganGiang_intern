CREATE DATABASE EduManager;
GO

USE EduManager;
GO

-- Bảng Subjects
CREATE TABLE Subjects (
    Id_Sub VARCHAR(10) PRIMARY KEY,
    Name_Sub NVARCHAR(100)
);

-- Bảng LearningStyle
CREATE TABLE LearningStyle (
    Id_LS INT PRIMARY KEY,
    Name_LS NVARCHAR(50)
);

-- Bảng EduProgram
CREATE TABLE EduProgram (
    Id_EP INT PRIMARY KEY IDENTITY(1,1),
    FK_Id_Sub VARCHAR(10),
    FK_Id_LS INT,
    NumHour INT,
    FOREIGN KEY (FK_Id_Sub) REFERENCES Subjects(Id_Sub),
    FOREIGN KEY (FK_Id_LS) REFERENCES LearningStyle(Id_LS)
);

-- Thêm dữ liệu vào bảng Subjects
INSERT INTO Subjects (Id_Sub, Name_Sub) VALUES 
('ITL112'	, N'Pháp luật đại cương'),
('IDEO111'	, N'Triết học Mác - Lênin'),
('IDEO122'	, N'Kinh tế chính trị Mác - Lênin'),
('SCSO232'	, N'Chủ nghĩa xã hội khoa học'),
('HCPV343'	, N'Lịch sử Đảng Cộng sản Việt Nam'),
('IDEO243'	, N'Tư tưởng Hồ Chí Minh');
GO

-- Thêm dữ liệu vào bảng LearningStyle
INSERT INTO LearningStyle (Id_LS, Name_LS) VALUES 
(1, N'Lý thuyết / lý luận'),
(2, N'Bài tập / Thảo luận'),
(3, N'Thực hành');

-- Thêm dữ liệu vào bảng EduProgram
INSERT INTO EduProgram (FK_Id_Sub, FK_Id_LS, NumHour) VALUES 
('ITL112', 1, 30), -- Pháp luật đại cương - Lý thuyết/lý luận - 60 giờ


SELECT * FROM Subjects
SELECT * FROM LearningStyle
SELECT * FROM EduProgram


DELETE FROM EduProgram
DELETE FROM Subjects
DELETE FROM LearningStyle

DROP TABLE EduProgram
DROP TABLE Subjects
DROP TABLE LearningStyle





Select 
	Id_Sub AS N'Mã môn học', 
	Name_Sub AS N'Tên môn học'
FROM Subjects



CREATE VIEW EduProgramDetails AS
SELECT 
    EP.Id_EP AS ID, 
    EP.FK_Id_Sub AS [Mã môn học], 
    S.Name_Sub AS [Tên môn học], 
    EP.FK_Id_LS AS [Mã hình thức học], 
    LS.Name_LS AS [Hình thức học], 
    EP.NumHour AS [Số tiết học]
FROM EduProgram AS EP
JOIN LearningStyle AS LS ON EP.FK_Id_LS = LS.Id_LS
JOIN Subjects AS S ON EP.FK_Id_Sub = S.Id_Sub;


Select * from EduProgramDetails


Select LS.Id_LS, LS.Name_LS, PD.FK_Id_LS from LearningStyle as LS, EduProgram AS PD
SELECT Id_LS, Name_LS FROM LearningStyle





-- Tạo view từ truy vấn PIVOT để tạo bảng ảo
CREATE VIEW EduProgramView AS
WITH SourceData AS (
    SELECT 
        FK_Id_Sub,
        Name_Sub,
        Name_LS,
        NumHour
    FROM 
        EduProgram
    JOIN 
        Subjects ON EduProgram.FK_Id_Sub = Subjects.Id_Sub
    JOIN 
        LearningStyle ON EduProgram.FK_Id_LS = LearningStyle.Id_LS
)

SELECT
    FK_Id_Sub AS "Mã môn học",
    Name_Sub AS "Tên môn học",
    [Lý thuyết / lý luận] AS "Lý thuyết",
    [Bài tập / Thảo luận] AS "Bài tập",
    [Thực hành] AS "Thực hành"
FROM
    SourceData
PIVOT (
    MAX(NumHour)
    FOR Name_LS IN ([Lý thuyết / lý luận], [Bài tập / Thảo luận], [Thực hành])
) AS PivotTable;


SELECT * FROM EduProgramView