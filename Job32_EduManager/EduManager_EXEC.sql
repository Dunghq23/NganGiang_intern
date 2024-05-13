CREATE PROCEDURE InsertUniqueSubjectAndEduProgram
    @Name_Sub NVARCHAR(100),
    @Sym_Sub NVARCHAR(10),
    @FK_Id_LS INT,
    @NumHour INT
AS
BEGIN
    DECLARE @new_id INT = 0
    DECLARE @exists BIT = 1

    -- Tìm mã Id_Sub duy nhất
    WHILE @exists = 1
    BEGIN
        IF EXISTS (SELECT 1 FROM Subjects WHERE Id_Sub = @new_id)
        BEGIN
            -- Nếu mã đã tồn tại, tăng mã Id_Sub
            SET @new_id = @new_id + 1
        END
        ELSE
        BEGIN
            -- Nếu mã không tồn tại, thoát khỏi vòng lặp
            SET @exists = 0
        END
    END

    -- Chèn dữ liệu vào bảng Subjects
    INSERT INTO Subjects (Id_Sub, Name_Sub, Sym_Sub)
    VALUES (@new_id, @Name_Sub, @Sym_Sub)

    -- Chèn dữ liệu vào bảng EduProgram
    INSERT INTO EduProgram (FK_Id_Sub, FK_Id_LS, NumHour)
    VALUES (@new_id, @FK_Id_LS, @NumHour)
END
GO

GO
-- Thủ tục tạo ID mới
CREATE PROCEDURE GetNextSubjectId
AS
BEGIN
    DECLARE @Id INT = 1  -- Khởi đầu với ID = 1

    -- Kiểm tra vòng lặp để xác định ID chưa tồn tại
    WHILE EXISTS (
        SELECT 1
        FROM Subjects
        WHERE Id_Sub = @Id
    )
    BEGIN
        SET @Id = @Id + 1  -- Tăng ID nếu ID hiện tại đã tồn tại
    END

    -- Trả về ID mới
    SELECT @Id AS NewSubjectId
END
GO

EXEC GetNextSubjectId
DROP PROCEDURE GetNextSubjectId
