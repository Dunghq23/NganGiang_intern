CREATE DATABASE TimeKeeping;
GO

USE TimeKeeping;
GO

CREATE TABLE departments
(
	[department_id] INT PRIMARY KEY IDENTITY(1, 1),
	[department_name] NVARCHAR(100) UNIQUE not null
)
GO

CREATE TABLE employees
(
    [employee_id] INT PRIMARY KEY IDENTITY(1, 1), -- Khóa chính
	[employee_username] VARCHAR (20) NOT NULL, -- Username của nhân viên 
    [employee_name] NVARCHAR(100) NOT NULL, -- Họ và tên nhân viên
    [email] NVARCHAR(100) NOT NULL UNIQUE, -- Email
    [phone_number] NVARCHAR(15), -- Số điện thoại
    [address] NVARCHAR(255), -- Địa chỉ
    [date_of_birth] DATE, -- Ngày sinh
    [gender] BIT, -- Giới tính [Nữ: 0; Nam: 1]
    [position] NVARCHAR(50), -- Chức vụ
    [fk_department_id] INT NOT NULL, -- Phòng ban (liên kết với bảng departments)
    [start_date] DATE, -- Ngày bắt đầu làm việc
    [salary] DECIMAL(18, 2), -- Mức lương
    [employment_status] BIT, -- Trạng thái công việc [Đã nghỉ việc: 0; Đang làm việc]
    [profile_picture] NVARCHAR(255), -- Đường dẫn ảnh đại diện
    [notes] NVARCHAR(MAX), -- Ghi chú
    [created_at] DATETIME DEFAULT GETDATE(), -- Ngày tạo
    [updated_at] DATETIME DEFAULT GETDATE(), -- Ngày cập nhật lần cuối
    
    -- Khóa ngoại
    FOREIGN KEY (fk_department_id) REFERENCES departments(department_id)
)
GO


CREATE TABLE timekeeping
(
    [timekeeping_id] INT PRIMARY KEY IDENTITY(1, 1),
    [fk_employee_id] INT NOT NULL,
    [check_in] DATETIME DEFAULT CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+07:00') AS DATETIME),
    [check_out] DATETIME,
    FOREIGN KEY (fk_employee_id) REFERENCES employees(employee_id)
);
GO


select * from employees