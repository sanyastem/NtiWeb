USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[TemplateGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TemplateGet]
	@CompanyId				uniqueidentifier = NULL
AS
BEGIN
SELECT	distinct T.Id AS Id, T.Name AS TemplateName
FROM	dbo.Document D (nolock)
		INNER JOIN dbo.Template T (nolock) ON T.Id = D.TemplateId
		INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND TT.CodeS = 'AGREEMENT-TEO'
WHERE	(
			D.CustomerCompanyId		= @CompanyId
		OR	D.PerformerCompanyId	= @CompanyId
		)
ORDER BY T.Name
END

	--SELECT 
	--	t.*,
	--	TemplateTypeName = rf.Name
	--FROM dbo.Template t
	--INNER JOIN NSI.Reference rf ON t.TemplateTypeId = rf.Id
	--WHERE (@TemplateId IS NULL OR t.Id = @TemplateId)
	--AND (t.IsDeleted = 0)
GO
