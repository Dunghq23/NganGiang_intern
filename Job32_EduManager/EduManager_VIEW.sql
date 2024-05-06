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


SELECT * FROM EduProgramView

DROP VIEW EduProgramView
