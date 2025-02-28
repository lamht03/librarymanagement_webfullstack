CREATE DATABASE DB_LibraryManagement
USE DB_LibraryManagement
ALTER DATABASE DB_LibraryManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE DB_LibraryManagement

--region Stored Procedure of Books
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Genre NVARCHAR(100) NOT NULL,
    PublishedDate DATE NOT NULL,
    TotalQuantity INT NOT NULL, -- Tổng số lượng sách
	Description NVARCHAR(MAX) NULL,
    CreatedDate DATE NOT NULL DEFAULT GETDATE()
);
GO	

CREATE PROCEDURE Books_GetAll
    @Title NVARCHAR(255) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Books
    WHERE @Title IS NULL OR Title LIKE '%' + @Title + '%';

    -- Lấy danh sách sách với thông tin số lượng
    SELECT 
        b.BookID,
        b.Title,
        b.Author,
        b.Genre,
        b.PublishedDate,
        b.TotalQuantity,
        b.Description,
        b.CreatedDate,
        -- Tính số lượng sách đang mượn từ bảng Transactions
        ISNULL((
            SELECT COUNT(*)
            FROM Transactions t
            WHERE t.BookID = b.BookID AND t.Status = 'Borrowed'
        ), 0) AS BorrowedQuantity,
        -- Tính số lượng sách còn lại
        b.TotalQuantity - ISNULL((
            SELECT COUNT(*)
            FROM Transactions t
            WHERE t.BookID = b.BookID AND t.Status = 'Borrowed'
        ), 0) AS AvailableQuantity
    FROM Books b
    WHERE @Title IS NULL OR b.Title LIKE '%' + @Title + '%'
    ORDER BY b.BookID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END;
GO

CREATE PROC Books_GetByID
    @BookID INT
AS
BEGIN
    SELECT 
        b.BookID,
        b.Title,
        b.Author,
        b.Genre,
        b.PublishedDate,
        b.TotalQuantity,
        b.Description,
        b.CreatedDate,
        -- Tính số lượng sách đang mượn từ bảng Transactions
        ISNULL((
            SELECT COUNT(*)
            FROM Transactions t
            WHERE t.BookID = b.BookID AND t.Status = 'Borrowed'
        ), 0) AS BorrowedQuantity,
        -- Tính số lượng sách còn lại
        b.TotalQuantity - ISNULL((
            SELECT COUNT(*)
            FROM Transactions t
            WHERE t.BookID = b.BookID AND t.Status = 'Borrowed'
        ), 0) AS AvailableQuantity
    FROM Books b
    ORDER BY b.BookID
END
GO

CREATE PROC Books_Insert
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Genre NVARCHAR(100),
    @PublishedDate DATE,
    @TotalQuantity INT
AS
BEGIN
    INSERT INTO Books (Title, Author, Genre, PublishedDate, TotalQuantity)
    VALUES (@Title, @Author, @Genre, @PublishedDate, @TotalQuantity);
END
GO

CREATE PROC Books_Update
    @BookID INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Genre NVARCHAR(100),
    @PublishedDate DATE,
    @TotalQuantity INT
AS
BEGIN
    UPDATE Books
    SET Title = @Title, Author = @Author, Genre = @Genre, PublishedDate = @PublishedDate, TotalQuantity = @TotalQuantity
    WHERE BookID = @BookID;
END
GO

CREATE PROC Books_Delete
    @BookID INT
AS
BEGIN
    DELETE FROM Books WHERE BookID = @BookID;
END
GO

--endregion

--region Stored Procedure of Transactions
CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    BookID INT NOT NULL,
    UserID INT NOT NULL,
    BorrowDate DATE NOT NULL DEFAULT GETDATE(), -- Ngày mượn
    DueDate DATE NOT NULL, -- Ngày phải trả
    ReturnDate DATE NULL, -- Ngày đã trả
    Status NVARCHAR(50) NOT NULL, -- Pending, Borrowed, Returned
	DepositAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Book FOREIGN KEY (BookID) REFERENCES Books(BookID),
    CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES Sys_User(UserID)
);
GO

CREATE PROC Transactions_GetAll
    @UserID INT = NULL,
    @Status NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    DECLARE @TotalRecords INT;

    SELECT @TotalRecords = COUNT(*)
    FROM Transactions
    WHERE (@UserID IS NULL OR UserID = @UserID)
      AND (@Status IS NULL OR Status = @Status);

    SELECT *
    FROM Transactions
    WHERE (@UserID IS NULL OR UserID = @UserID)
      AND (@Status IS NULL OR Status = @Status)
    ORDER BY TransactionID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT @TotalRecords AS TotalRecords;
END
GO

CREATE PROC Transactions_GetByID
    @TransactionID INT
AS
BEGIN
    SELECT * 
    FROM Transactions 
    WHERE TransactionID = @TransactionID;
END
GO

CREATE PROC Transactions_Insert
    @BookID INT,
    @UserID INT,
	@BorrowDate DATE,
    @DueDate DATE,
	@DepositAmount DECIMAL(18,2),
	@Status NVARCHAR(100)
AS
BEGIN
    INSERT INTO Transactions (BookID, UserID, BorrowDate, DueDate,DepositAmount, Status)
    VALUES (@BookID, @UserID, @BorrowDate, @DueDate, @DepositAmount, @Status);
END;
GO


CREATE PROC Transactions_Update
    @TransactionID INT,
    @Status NVARCHAR(100),
	@BorrowDate DATE , -- Ngày mượn
    @DueDate DATE , -- Ngày phải trả
    @ReturnDate DATE  -- Ngày đã trả
AS
BEGIN
    UPDATE Transactions
    SET Status = @Status, UserID = @UserID
    WHERE TransactionID = @TransactionID;
END
GO


--endregion

--region Stored Procedure of Payments
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    TransactionID INT NOT NULL,
    UserID INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL, -- Số tiền thanh toán
    PaymentDate DATE NOT NULL DEFAULT GETDATE(), -- Ngày thanh toán
    PaymentMethod NVARCHAR(50) NOT NULL, -- Tiền mặt/Chuyển khoản
    PaymentStatus NVARCHAR(50) NOT NULL, -- Pending, Completed
    CONSTRAINT FK_Transaction FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID),
    CONSTRAINT FK_UserPayment FOREIGN KEY (UserID) REFERENCES Sys_User(UserID)
);
GO

CREATE PROC Payments_GetAll
    @UserID INT = NULL,
    @PaymentStatus NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    DECLARE @TotalRecords INT;

    SELECT @TotalRecords = COUNT(*)
    FROM Payments
    WHERE (@UserID IS NULL OR UserID = @UserID)
      AND (@PaymentStatus IS NULL OR PaymentStatus = @PaymentStatus);

    SELECT *
    FROM Payments
    WHERE (@UserID IS NULL OR UserID = @UserID)
      AND (@PaymentStatus IS NULL OR PaymentStatus = @PaymentStatus)
    ORDER BY PaymentID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT @TotalRecords AS TotalRecords;
END
GO

CREATE PROC Payments_GetByID
    @PaymentID INT
AS
BEGIN
    SELECT * 
    FROM Payments 
    WHERE PaymentID = @PaymentID;
END
GO

CREATE PROC Payments_Insert
    @TransactionID INT,
    @UserID INT,
    @Amount DECIMAL(18, 2),
    @PaymentMethod NVARCHAR(50)
AS
BEGIN
    INSERT INTO Payments (TransactionID, UserID, Amount, PaymentMethod)
    VALUES (@TransactionID, @UserID, @Amount, @PaymentMethod);
END
GO

CREATE PROCEDURE UpdatePaymentStatus
    @PaymentID INT,
    @PaymentStatus NVARCHAR(50)
AS
BEGIN
    UPDATE Payments
    SET PaymentStatus = @PaymentStatus
    WHERE PaymentID = @PaymentID;
END;
GO

--endregion

--region Authorization Mangement System
--region Stored procedures of Users
CREATE TABLE Sys_User (
	UserID INT PRIMARY KEY IDENTITY(1,1),
	UserName NVARCHAR(50),
	FullName NVARCHAR (100) null,
	Email NVARCHAR(50),
	Password NVARCHAR(100),
	Status BIT ,
	Note NVARCHAR(100),
);
GO


-- Get All Users
CREATE PROCEDURE UMS_GetListPaging
    @UserName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_User
    WHERE @UserName IS NULL OR UserName LIKE '%' + @UserName + '%';

    -- Return data for the current page
    SELECT 
        UserID,
        UserName,
		FullName,
        Email,       -- Added Email field
        Password,
        Status,      -- Added Status field
        Note		 -- Added Note field
    FROM Sys_User
    WHERE @UserName IS NULL OR UserName LIKE '%' + @UserName + '%'
    ORDER BY UserID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO

-- Get User by UserID
CREATE PROCEDURE UMS_GetByID
    @UserID int
AS
BEGIN
    SELECT * FROM Sys_User WHERE UserID = @UserID;
END
GO

--Get by UserName
CREATE PROC UMS_GetByUserName
	@UserName NVARCHAR(50)
AS
BEGIN
	SELECT * FROM Sys_User su WHERE su.UserName = @UserName
END	


GO

CREATE PROCEDURE UMS_GetByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Sys_User
    WHERE Email = @Email;
END;
GO

-- Create User
CREATE PROCEDURE UMS_Create
    @UserName NVARCHAR(50),
	@FullName NVARCHAR (100),
    @Email NVARCHAR(50),
    @Password NVARCHAR(100),
    @Status BIT,
    @Note NVARCHAR(100) = 'Regular user'
AS
BEGIN
    -- Insert new user record
    INSERT INTO Sys_User (UserName, Email, Password, Status, Note)
    VALUES (@UserName, @Email, @Password, @Status, @Note);
END
GO

-- Update User
ALTER PROCEDURE UMS_Update
    @UserID INT,
    @UserName NVARCHAR(50),
	@FullName NVARCHAR(100),
    @Email NVARCHAR(50),
    @Password NVARCHAR(100),
    @Status BIT,
    @Note NVARCHAR(100)
AS
BEGIN
    -- Update existing user record
    UPDATE Sys_User
    SET 
        UserName = @UserName,
        Email = @Email,
        Password = @Password,
        Status = @Status,
        Note = @Note,
		FullName = @FullName
    WHERE UserID = @UserID;
END
GO

-- Delete User
ALTER PROCEDURE [dbo].[UMS_Delete]
    @UserID INT
AS
BEGIN
	DELETE FROM Session
	WHERE UserID = @UserID
    DELETE FROM Sys_UserInGroup
    WHERE UserID = @UserID;
    DELETE FROM Sys_User
	WHERE UserID = @UserID;
END
GO

-- Veryfy Login
create PROCEDURE VerifyLogin
    @UserName NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Sys_User
    WHERE UserName = @UserName AND Password = @Password;
END
GO
--endregion

--region Stored procedures of UserGroups
CREATE TABLE Sys_Group (
    GroupID INT PRIMARY KEY IDENTITY(1,1),
    GroupName NVARCHAR(50),
    Description NVARCHAR(100)
);
GO
-- Get All Group
create PROCEDURE GMS_GetListPaging
    @GroupName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_Group sug
    WHERE @GroupName IS NULL OR GroupName LIKE '%' + @GroupName + '%';

    -- Return data for the current page
    SELECT 
        sug.GroupID,
        sug.GroupName,
        sug.Description  
    FROM Sys_Group sug
    WHERE @GroupName IS NULL OR GroupName LIKE '%' + @GroupName + '%'
    ORDER BY sug.GroupID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO
-- Get Group By ID
CREATE PROC GMS_GetByID
	@GroupID INT 
AS 
BEGIN
	SELECT * FROM Sys_Group sg WHERE  sg.GroupID = @GroupID
END
GO

-- Create Group 
CREATE PROCEDURE GMS_Create
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Insert a new record into Sys_Group
    INSERT INTO Sys_Group (GroupName, Description)
    VALUES (@GroupName, @Description);
END
GO

-- Update Group
CREATE PROCEDURE GMS_Update
    @GroupID INT,
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Update the existing record in Sys_Group
    UPDATE Sys_Group
    SET GroupName = @GroupName,
        Description = @Description
    WHERE GroupID = @GroupID;
END
GO

-- Delete Group
CREATE PROCEDURE GMS_Delete
    @GroupID INT
AS
BEGIN
    -- Delete the record from Sys_Group
    DELETE FROM Sys_Group
    WHERE GroupID = @GroupID;
END
GO
--endregion

--region Stored procedures of Function
CREATE TABLE Sys_Function (
    FunctionID INT PRIMARY KEY IDENTITY(1,1),
    FunctionName NVARCHAR(50),
    Description NVARCHAR(100),
);
GO
-- GetListPaging of Function
CREATE PROCEDURE FMS_GetListPaging
    @FunctionName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_Function sf
    WHERE @FunctionName IS NULL OR FunctionName LIKE '%' + @FunctionName + '%';

    -- Return data for the current page
    SELECT 
        FunctionID,
        FunctionName,
        Description      
       
    FROM Sys_Function sf
    WHERE @FunctionName IS NULL OR FunctionName LIKE '%' + @FunctionName + '%'
    ORDER BY FunctionID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO

-- Get Function by ID
create PROC FMS_GetByID
	@FunctionID int
AS
BEGIN
	SELECT * FROM Sys_Function sf WHERE sf.FunctionID = @FunctionID
END
GO

-- Create Function
CREATE PROCEDURE FMS_Create
    @FunctionName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Insert a new record into Sys_Function
    INSERT INTO Sys_Function (FunctionName, Description)
    VALUES (@FunctionName, @Description);
END
GO

-- Update Function
CREATE PROCEDURE FMS_Update
    @FunctionID INT,
    @FunctionName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Update the existing record in Sys_Function
    UPDATE Sys_Function
    SET FunctionName = @FunctionName,
        Description = @Description
    WHERE FunctionID = @FunctionID;
END
GO

-- Delete Function
CREATE PROCEDURE FMS_Delete
    @FunctionID INT
AS
BEGIN
    -- Delete the record from Sys_Function
    DELETE FROM Sys_Function
    WHERE FunctionID = @FunctionID;
END
GO


--endregion

--region Stored procedures of UserInGroup
CREATE TABLE Sys_UserInGroup (
	UserInGroupID INT PRIMARY KEY IDENTITY(1,1) ,
    UserID INT NOT NULL,
    GroupID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Sys_User(UserID),
    FOREIGN KEY (GroupID) REFERENCES Sys_Group(GroupID)
);
GO

CREATE PROCEDURE UIG_GetAll
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup;
END;
GO

CREATE PROCEDURE UIG_GetByGroupID
    @GroupID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE GroupID = @GroupID;
END;
GO	

CREATE PROCEDURE UIG_GetByUserID
    @UserID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE UserID = @UserID;
END;
GO	

CREATE PROCEDURE UIG_GetByID
    @UserInGroupID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE UserInGroupID = @UserInGroupID;
END;
GO

-- Add User to Group
CREATE PROC UIG_Create  
	@UserID INT ,
	@GroupID INT
AS
BEGIN
	INSERT INTO Sys_UserInGroup (UserID, GroupID)
	VALUES (@UserID,@GroupID);
END
GO

CREATE PROCEDURE UIG_Update
    @UserInGroupID INT,
    @UserID INT,
    @GroupID INT
AS
BEGIN
    UPDATE Sys_UserInGroup
    SET UserID = @UserID, GroupID = @GroupID
    WHERE UserInGroupID = @UserInGroupID;
END;
GO

create PROCEDURE UIG_Delete
    @UserInGroupID INT
AS
BEGIN
    DELETE FROM Sys_UserInGroup
    WHERE UserInGroupID = @UserInGroupID;
END;
GO	
--endregion

--region Stored procedures of FunctionInGroup
CREATE TABLE Sys_FunctionInGroup (
	FunctionInGroupID INT PRIMARY KEY IDENTITY(1,1),
    FunctionID INT,
    GroupID INT,
	Permission INT NOT NULL, 
    FOREIGN KEY (FunctionID) REFERENCES Sys_Function(FunctionID),
    FOREIGN KEY (GroupID) REFERENCES Sys_Group(GroupID)
);
GO

CREATE PROCEDURE FIG_GetAll
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup;
END
GO

CREATE PROCEDURE FIG_GetByGroupID
    @GroupID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE GroupID = @GroupID;
END
GO

CREATE PROCEDURE FIG_GetByFunctionID
    @FunctionID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE FunctionID = @FunctionID;
END
GO

CREATE PROCEDURE FIG_GetByID
    @FunctionInGroupID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE FunctionInGroupID = @FunctionInGroupID;
END
GO

CREATE PROCEDURE FIG_Create
    @FunctionID INT,
    @GroupID INT,
    @Permission INT
AS
BEGIN
    INSERT INTO Sys_FunctionInGroup (FunctionID, GroupID, Permission)
    VALUES (@FunctionID, @GroupID, @Permission);
END;
GO	

CREATE PROCEDURE FIG_Update
    @FunctionID INT,
    @GroupID INT,
    @Permission INT
AS
BEGIN
    UPDATE Sys_FunctionInGroup
    SET Permission = @Permission
    WHERE FunctionID = @FunctionID AND GroupID = @GroupID;
END;
GO	

CREATE PROCEDURE FIG_Delete
    @FunctionInGroupID INT
AS
BEGIN
    DELETE FROM Sys_FunctionInGroup
    WHERE FunctionInGroupID = @FunctionInGroupID;
END
GO	

CREATE PROCEDURE FIG_GetAllUserPermissions
    @UserName NVARCHAR(50),
    @FunctionName NVARCHAR(50)
AS
BEGIN
    SELECT Permission
    FROM Sys_FunctionInGroup fg
    INNER JOIN Sys_Function f ON fg.FunctionID = f.FunctionID
    INNER JOIN Sys_UserInGroup ug ON fg.GroupID = ug.GroupID
    INNER JOIN Sys_User u ON ug.UserID = u.UserID
    WHERE u.UserName = @UserName AND f.FunctionName = @FunctionName
END
GO	

CREATE PROCEDURE FIG_GetAllUserFunctionsAndPermissions
    @UserName NVARCHAR(50)
AS
BEGIN
    SELECT f.FunctionName, fg.Permission
    FROM Sys_FunctionInGroup fg
    INNER JOIN Sys_Function f ON fg.FunctionID = f.FunctionID
    INNER JOIN Sys_UserInGroup ug ON fg.GroupID = ug.GroupID
    INNER JOIN Sys_User u ON ug.UserID = u.UserID
    WHERE u.UserName = @UserName
END
GO	

--endregion

--region Stored procedures of RefreshToken
CREATE TABLE Session
(
    SessionID INT IDENTITY PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Sys_User(UserID),
    RefreshToken NVARCHAR(255) NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,   -- Đánh dấu refresh token này đã bị thu hồi hay chưa
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
);
GO


CREATE PROCEDURE CreateSession
    @UserID INT,
    @RefreshToken NVARCHAR(255),
    @ExpiryDate DATETIME
AS
BEGIN
    -- Tạo phiên (Session) mới cho người dùng với refresh token
    INSERT INTO Session (UserID, RefreshToken, ExpiryDate, IsRevoked, CreatedAt)
    VALUES (@UserID, @RefreshToken, @ExpiryDate, 0, GETDATE());
END
GO


CREATE PROCEDURE GetSessionByRefreshToken
    @RefreshToken NVARCHAR(255)
AS
BEGIN
    SELECT SessionID, UserID, RefreshToken, ExpiryDate, IsRevoked, CreatedAt
    FROM Session
    WHERE RefreshToken = @RefreshToken AND IsRevoked = 0 AND ExpiryDate > GETDATE();
END
GO

CREATE PROCEDURE UpdateSessionRefreshToken
    @SessionID INT,
    @NewRefreshToken NVARCHAR(255),
    @NewExpiryDate DATETIME
AS
BEGIN
    UPDATE Session
    SET RefreshToken = @NewRefreshToken, ExpiryDate = @NewExpiryDate, CreatedAt = GETDATE()
    WHERE SessionID = @SessionID AND IsRevoked = 0;
END
GO


CREATE PROCEDURE RevokeSession
    @SessionID INT
AS
BEGIN
    UPDATE Session
    SET IsRevoked = 1, CreatedAt = GETDATE()
    WHERE SessionID = @SessionID;
END
GO

CREATE PROCEDURE RevokeAllSessions
    @UserID INT
AS
BEGIN
    UPDATE Session
    SET IsRevoked = 1, CreatedAt = GETDATE()
    WHERE UserID = @UserID AND IsRevoked = 0;
END
GO

CREATE PROCEDURE DeleteAllSessions
    @UserID INT
AS
BEGIN
    DELETE FROM Session
    WHERE UserID = @UserID;
END
GO



--endregion
--endregion
--region Insert records into  Authorization Management
-- Thêm người dùng
INSERT INTO Sys_User (UserName, Email, Password, Status, Note)
VALUES
('admin', 'admin@example.com', 'admin', 1, 'Admin user'),
('user1', 'user1@example.com', 'user1', 1, 'Regular user');
GO	

--Thêm chức năng
INSERT INTO Sys_Function (FunctionName, Description)
	VALUES ('ManageUsers', N'Quản lý người dùng'),
	('ManageMonumentRanking', N'Quản lý di tích xếp hạng'),
	('ManageUnitofMeasure', N'Quản lý đơn vị tính'),
	('ManageReportingPeriod', N'Quản lý kỳ báo cáo'),
	('ManageTypeofMonument', N'Quản lý loại di tích'),
	('ManageFormType', N'Quản lý mẫu phiếu'),
	('ManageCriteria', N'Quản lý tiêu chí'),
	('ManageTarget', N'Quản lý chỉ tiêu'),
	('ManageReportForm', N'Quản lý mẫu phiếu báo cáo'),
	('ManageAuthorization', N'Quản lý ủy quyền');
GO
SELECT * FROM Sys_FunctionInGroup

-- Thêm nhóm
INSERT INTO Sys_Group (GroupName, Description)
VALUES
('AdminGroup', N'Nhóm quản trị'),
('UserGroup', N'Nhóm người dùng');
GO
-- Thêm người dùng vào nhóm
INSERT INTO Sys_UserInGroup (UserID, GroupID)
VALUES
(1, 1),  -- admin vào AdminGroup
(2, 2);  -- user1 vào UserGroup
GO	


-- Thêm chức năng vào nhóm và phân quyền
INSERT INTO Sys_FunctionInGroup (FunctionID, GroupID, Permission)
VALUES
(1, 1, 15),
(1, 2, 0),
(2, 1, 15),
(2, 2, 7),
(3, 1, 15),
(3, 2, 7),
(4, 1, 15),
(4, 2, 7),
(5, 1, 15),
(5, 2, 7),
(6, 1, 15),
(6, 2, 7),
(7, 1, 15),
(7, 2, 7),
(8, 1, 15),
(8, 2, 7),
(9,1,15),
(9,2,7),
(10,1,15),
(10,2,0)
GO  
--endregion

INSERT INTO Sys_Function (FunctionName, Description)
	VALUES ('ManageBooks', N'Quản lý sách'),
	('ManageTransactions', N'Quản lý giao dịch'),
	('ManagePayments', N'Quản lý giao dịch')

INSERT INTO Sys_FunctionInGroup (FunctionID, GroupID, Permission)
VALUES
(11, 1, 15),
(11, 2, 15),
(12, 1, 15),
(12, 2, 15),
(13, 1, 15),
(13, 2, 15)



EXEC Books_Insert @Title = 'Introduction to Computer Science', @Author = 'John Doe', @Genre = 'Computer Science', @PublishedDate = '2020-01-15', @TotalQuantity = 50;
EXEC Books_Insert @Title = 'Advanced Programming', @Author = 'Jane Smith', @Genre = 'Computer Science', @PublishedDate = '2019-06-25', @TotalQuantity = 40;
EXEC Books_Insert @Title = 'Data Structures and Algorithms', @Author = 'Mark Johnson', @Genre = 'Computer Science', @PublishedDate = '2018-09-10', @TotalQuantity = 60;
EXEC Books_Insert @Title = 'Artificial Intelligence: A Modern Approach', @Author = 'Stuart Russell', @Genre = 'Artificial Intelligence', @PublishedDate = '2015-11-05', @TotalQuantity = 30;
EXEC Books_Insert @Title = 'Machine Learning Yearning', @Author = 'Andrew Ng', @Genre = 'Machine Learning', @PublishedDate = '2018-05-23', @TotalQuantity = 45;
EXEC Books_Insert @Title = 'The Pragmatic Programmer', @Author = 'Andrew Hunt, David Thomas', @Genre = 'Software Engineering', @PublishedDate = '1999-10-30', @TotalQuantity = 70;
EXEC Books_Insert @Title = 'Clean Code: A Handbook of Agile Software Craftsmanship', @Author = 'Robert C. Martin', @Genre = 'Software Engineering', @PublishedDate = '2008-08-01', @TotalQuantity = 80;
EXEC Books_Insert @Title = 'The Mythical Man-Month', @Author = 'Frederick P. Brooks', @Genre = 'Software Engineering', @PublishedDate = '1975-10-15', @TotalQuantity = 25;
EXEC Books_Insert @Title = 'Operating System Concepts', @Author = 'Abraham Silberschatz', @Genre = 'Operating Systems', @PublishedDate = '2019-01-12', @TotalQuantity = 50;
EXEC Books_Insert @Title = 'Computer Networking: A Top-Down Approach', @Author = 'James Kurose, Keith Ross', @Genre = 'Networking', @PublishedDate = '2020-07-01', @TotalQuantity = 40;
