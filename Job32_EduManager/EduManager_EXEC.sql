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


EXEC GetNextSubjectId
DROP PROCEDURE GetNextSubjectId
