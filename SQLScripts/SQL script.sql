CREATE DATABASE MBAndW;
GO
USE MBAndW
GO
CREATE TABLE Task (
    ID [int] IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    Due_Date DATE NOT NULL,
	Start_Date DATE NOT NULL,
	End_Date DATE NOT NULL,
	Priority [int] NOT NULL Default 3 ,
	Status [int] NOT NULL Default 1,
	CONSTRAINT chk_Priority CHECK (Priority IN (1,2,3)),
	CONSTRAINT chk_Status CHECK (Status IN (1,2,3)),
	CONSTRAINT chk_End_Date CHECK (Start_Date <= End_Date)
);
GO

CREATE PROCEDURE [dbo].[SP_Add_Task] 
       @name NVARCHAR(255), 
	   @description NVARCHAR(255), 
       @duedate DATE, 
	   @startdate DATE,
	   @enddate DATE,
       @priority int, 
       @status int,  
	   @Msg nvarchar(MAX)=null OUTPUT
AS 
BEGIN TRY
     SET NOCOUNT ON 
     INSERT INTO [dbo].[Task]
          (                    
       Name, 
	   Description, 
       Due_Date, 
	   Start_Date,
	   End_Date,
       Priority, 
       Status
          ) 
     VALUES 
          ( 
       @name, 
	   @description, 
       @duedate, 
	   @startdate,
	   @enddate,
       @priority, 
       @status          
          ) 		 

END TRY
BEGIN CATCH
    SET @Msg=ERROR_MESSAGE()
END CATCH
GO

CREATE PROCEDURE [dbo].[SP_Update_Task]
(
@id int,
@name NVARCHAR(255), 
	   @description NVARCHAR(255), 
       @duedate DATE, 
	   @startdate DATE,
	   @enddate DATE,
       @priority int, 
       @status int,  
	   @Msg nvarchar(MAX)=null OUTPUT
)
AS
BEGIN TRY
UPDATE [dbo].[Task]
SET
Name = @name, 
	   Description = @description, 
       Due_Date = @duedate, 
	   Start_Date = @startdate,
	   End_Date = @enddate,
       Priority =@priority , 
       Status = @status

WHERE ID=@id

END TRY
BEGIN CATCH

    SET @Msg=ERROR_MESSAGE()

END CATCH

GO

-- Optional

CREATE PROCEDURE [dbo].[SP_CheckTasksCountByStatus]
(
@dueDate Date,
@isValid bit OUTPUT
)

AS
BEGIN TRY
SELECT @isValid= (CASE WHEN (Count(*) >= 100) THEN 0  ELSE 1 END)
FROM dbo.Task
where status != 3 and priority = 1 and Due_Date = @dueDate 
END TRY
BEGIN CATCH   

END CATCH

GO

