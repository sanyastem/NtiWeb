USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeInsert] (
		@UserId				uniqueidentifier,
		@CompanyId			uniqueidentifier,
		@CreatedBy			uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Employee (
		Id,
		UserId,
		CompanyId,
		CreatedBy,
		CreatedOn,
		IsDeleted
	)

	VALUES (
		NEWID(),
		@UserId,
		@CompanyId,
		@CreatedBy,
		GetDate(),
		0
	)
END

GO
