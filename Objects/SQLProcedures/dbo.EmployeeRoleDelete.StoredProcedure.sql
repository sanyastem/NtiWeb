USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeRoleDelete]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeRoleDelete] (
		@EmployeeId	uniqueidentifier,
		@RoleId		uniqueidentifier,
		@UpdatedBy	uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.EmployeeRole SET IsDeleted = 1, UpdatedBy = @UpdatedBy,  UpdatedOn = Getdate()
	WHERE EmployeeId = @EmployeeId AND RoleId = @RoleId
END

GO
