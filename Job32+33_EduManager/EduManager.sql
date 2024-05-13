ÔCREATE DATABASE EduManager;
GO

USE EduManager;
GO

-- Bảng Subjects
CREATE TABLE Subjects (
    Id_Sub INT PRIMARY KEY,
    Name_Sub NVARCHAR(100),
    Sym_Sub VARCHAR(10)
);

-- Bảng LearningStyle
CREATE TABLE LearningStyle (
    Id_LS INT PRIMARY KEY,
    Name_LS NVARCHAR(50)
);

-- Bảng EduProgram
CREATE TABLE EduProgram (
    Id_EP INT PRIMARY KEY IDENTITY(1,1),
    FK_Id_Sub INT,
    FK_Id_LS INT,
    NumHour INT,
    FOREIGN KEY (FK_Id_Sub) REFERENCES Subjects(Id_Sub),
    FOREIGN KEY (FK_Id_LS) REFERENCES LearningStyle(Id_LS)
);

-- Bảng LessonSubject
CREATE TABLE LessonSub (
	Id_Les INT PRIMARY KEY IDENTITY(1,1),
	Les_Unit NVARCHAR(10),
	Les_Name NVARCHAR(200),
	FK_Id_Sub INT,
    FK_Id_LS INT,
    NumHour INT DEFAULT 0,
    FOREIGN KEY (FK_Id_Sub) REFERENCES Subjects(Id_Sub),
    FOREIGN KEY (FK_Id_LS) REFERENCES LearningStyle(Id_LS)
);

INSERT INTO LearningStyle VALUES
(1, N'Lý thuyết / Lý luận'),
(2, N'Bài tập / Thảo luận'),
(3, N'Thực hành')


SELECT * FROM Subjects
SELECT * FROM LearningStyle
SELECT * FROM EduProgram
SELECT * FROM LessonSub



DELETE FROM EduProgram
DELETE FROM Subjects
DELETE FROM LearningStyle
DELETE FROM LessonSub


DROP TABLE LessonSub
DROP TABLE EduProgram
DROP TABLE Subjects
DROP TABLE LearningStyle
