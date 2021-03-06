USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClientsGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClientsGet]
AS
BEGIN
SELECT	distinct C.Id, C.ShortName
FROM	dbo.Document D (nolock)
		INNER JOIN dbo.Template T ON T.Id = D.TemplateId
		INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND TT.CodeS = 'ACCOUNT-TEO'
		INNER JOIN dbo.Company C (nolock) ON C.Id = D.CustomerCompanyId
ORDER BY C.ShortName
END
GO
