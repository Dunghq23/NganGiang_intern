USE TimeKeeping;

INSERT INTO departments ([department_name]) values
(N'Công nghệ thông tin'), 
(N'Kinh doanh'), 
(N'Tài chính - Kế toán'), 
('Marketing');

SELECT * FROM departments;
--DELETE FROM Departments;

--GO
--DBCC CHECKIDENT ('Departments', RESEED, 0);
--GO

--SELECT @@SERVERNAME; -- get your server name

--ALTER AUTHORIZATION ON DATABASE::[TimeKeeping] TO [REALTUN\Administrator];

--INSERT INTO Employees ([employee_name], [fk_department_id]) values
--(N'Đỗ Hữu Tuấn', 1), (N'Lê Đình Tú', 1), (N'Nguyễn Kim Thi', 1), (N'Hạ Quang Dũng', 1), 
--(N'Phạm Trọng Nghĩa', 1), (N'Đặng Hải Sơn', 1), (N'Lê Thu Trang', 1), (N'Phạm Quỳnh Anh', 1);

SELECT * FROM employees;



INSERT INTO employees 
(
    employee_username, 
    employee_name, 
    email, 
    phone_number, 
    address, 
    date_of_birth, 
    gender, 
    position, 
    fk_department_id, 
    start_date, 
    salary, 
    employment_status, 
    profile_picture, 
    notes
)
VALUES 
(
    N'dunghq23', 
    N'Hạ Quang Dũng', 
    'haquangdung18092003@gmail.com', 
    N'0393049255', 
    N'Tổ 8 - Thị trấn Chi Đông - Mê Linh - Hà Nội', 
    '2003-09-18', 
    1, 
    N'Thực tập sinh', 
    1, 
    '2023-09-15', 
    100.00, 
    1, 
    N'https://absvietnam.com/wp-content/uploads/2024/03/Nhat-Ban-dat-nuoc-mat-troi-moc-1.jpg', 
    N'Thực tập mảng .NET, AI, Web, Android,..'
)