USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementNumberListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AgreementNumberListGet]
	@CompanyId uniqueidentifier				-- Guid компании
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	distinct A.Id AS AgreementId, CASE WHEN @CompanyId = A.CustomerCompanyId THEN A.CustomerName ELSE A.PerformerName END AS AgreementName
	FROM	dbo.Document C (nolock)
		INNER JOIN dbo.Template T (nolock) ON T.Id = C.TemplateId
		INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND TT.CodeS = 'CLAIM-TEO'
		INNER JOIN dbo.Document2Document D2D (nolock) ON D2D.ChildId = C.Id
		INNER JOIN dbo.Document A (nolock) ON A.Id = D2D.ParentId
	WHERE	(
			A.CustomerCompanyId		= @CompanyId
		OR	A.PerformerCompanyId	= @CompanyId
		)
	ORDER BY AgreementName
END

GO
