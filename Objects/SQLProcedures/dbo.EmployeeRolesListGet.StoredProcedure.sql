USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeRolesListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeRolesListGet](
		@CompanyId			uniqueidentifier,					--Id компании
		@UserId				uniqueidentifier					--Id пользователя
		)
AS

BEGIN
	SET NOCOUNT ON;

	SELECT	Id				=R.Id,
			Name			=R.Name,
			EmployeeId		=E.Id

	FROM Employee E
		INNER JOIN dbo.EmployeeRole ER (nolock) ON E.Id = ER.EmployeeId
		INNER JOIN dbo.Role R (nolock) ON  ER.RoleId = R.Id

	WHERE E.IsDeleted = 0 AND ER.IsDeleted = 0 AND E.CompanyId = @CompanyId AND E.UserId = @UserId
END

GO
