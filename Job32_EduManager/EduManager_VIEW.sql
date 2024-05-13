-- Tạo view từ truy vấn PIVOT để tạo bảng ảo
CREATE VIEW EduProgramView AS
WITH SourceData AS (
    SELECT 
        FK_Id_Sub,
        Name_Sub,
		Sym_Sub,
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
    Sym_Sub AS "Ký hiệu",
    [Lý thuyết / lý luận] AS "Lý thuyết",
    [Bài tập / Thảo luận] AS "Bài tập",
    [Thực hành] AS "Thực hành"
FROM
    SourceData
PIVOT (
    MAX(NumHour)
    FOR Name_LS IN ([Lý thuyết / lý luận], [Bài tập / Thảo luận], [Thực hành])
) AS PivotTable;
GO

SELECT [Lý thuyết] FROM EduProgramView
WHERE [Ký hiệu] = 'H1' 

DROP VIEW EduProgramView
GO


-- Tạo view Quản lý phân phối chương trình học
CREATE VIEW LessonSubjectView AS
WITH SourceData AS (
    SELECT
		Subjects.Sym_Sub,
        Les_Unit,
		Les_Name,
        Name_LS,
        NumHour
    FROM 
        LessonSub
    JOIN 
        Subjects ON LessonSub.FK_Id_Sub = Subjects.Id_Sub
    JOIN 
        LearningStyle ON LessonSub.FK_Id_LS = LearningStyle.Id_LS
)
SELECT
	Sym_Sub AS "Ký hiệu môn học",
    Les_Unit AS "Bài học",
    Les_Name AS "Tên bài học",
    [Lý thuyết / lý luận] AS "Lý thuyết",
    [Bài tập / Thảo luận] AS "Bài tập",
    [Thực hành] AS "Thực hành"
FROM
    SourceData
PIVOT (
    MAX(NumHour)
    FOR Name_LS IN ([Lý thuyết / lý luận], [Bài tập / Thảo luận], [Thực hành])
) AS PivotTable;
GO

SELECT * FROM LessonSubjectView
WHERE "Ký hiệu môn học" = 'H2'
SELECT * FROM LessonSubjectView
WHERE [Ký hiệu môn học] = 'H1' AND [Bài học] = 'Bài 01'

DROP VIEW LessonSubjectView
GO

-- Tạo view liệt kê những môn học chưa được phân phối chương trình
CREATE VIEW UnassignedSubjectsView AS
SELECT s.Id_Sub, s.Name_Sub
FROM Subjects s LEFT JOIN LessonSub ls ON s.Id_Sub = ls.FK_Id_Sub
WHERE ls.Id_Les IS NULL;
GO

SELECT * FROM UnassignedSubjectsView;
DROP VIEW UnassignedSubjectsView
GO
