USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[RolePermissionDelete]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RolePermissionDelete]
		@RoleId					uniqueidentifier,	
		@PermissionId			uniqueidentifier,
		@UpdatedBy				uniqueidentifier

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.RolePermission SET 
	IsDeleted = 1,
	UpdatedOn = GetDate(),
	UpdatedBy = @UpdatedBy

	WHERE RoleId = @RoleId AND PermissionId = @PermissionId
END

GO
