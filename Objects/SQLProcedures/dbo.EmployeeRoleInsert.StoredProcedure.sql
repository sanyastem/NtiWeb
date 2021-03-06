USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeRoleInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeRoleInsert] (
		@CompanyId			uniqueidentifier,
		@UserId				uniqueidentifier,
		@RoleId				uniqueidentifier,
		@CreatedBy			uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EmployeeId uniqueidentifier = (SELECT top 1 E.Id FROM dbo.Employee E WHERE E.CompanyId = @CompanyId AND E.UserId = @UserId)

	INSERT INTO dbo.EmployeeRole (
		Id,
		EmployeeId,
		RoleId,
		CreatedBy,
		CreatedOn,
		IsDeleted
	)

	VALUES (
		NEWID(),
		@EmployeeId,
		@RoleId,
		@CreatedBy,
		GetDate(),
		0
	)
	
END

GO
