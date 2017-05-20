USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserCompaniesListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserCompaniesListGet] (
		@UserId		uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CompanyId = E.CompanyId

	FROM dbo.Employee E

	WHERE E.UserId = @UserId AND E.IsDeleted = 0

END

GO
