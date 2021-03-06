USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[RolePermissionInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RolePermissionInsert] (
		@RoleId					uniqueidentifier,
		@PermissionId			uniqueidentifier,
		@CreatedBy				uniqueidentifier
)
		
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.RolePermission(
		RoleId,
		PermissionId,
		CreatedBy,
		CreatedOn
	)

	VALUES (
		@RoleId,
		@PermissionId,
		@CreatedBy,
		GetDate()
	)
END

GO
