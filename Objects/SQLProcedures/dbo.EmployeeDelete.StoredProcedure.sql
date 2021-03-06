USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeDelete]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeDelete] (
		@UserId		uniqueidentifier,
		@CompanyId	uniqueidentifier,
		@UpdatetBy	uniqueidentifier		--пользователь вносящий изменения
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Employee SET IsDeleted = 1, UpdatedBy = @UpdatetBy, UpdatedOn = Getdate()
	WHERE UserId = @UserId AND CompanyId = @CompanyId

END

GO
