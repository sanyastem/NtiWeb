USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[EmployeeGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeGet]
	@CompanyId				uniqueidentifier = NULL
AS
BEGIN
SELECT distinct Q1.AgrOwnerId, Q1.AgrOwnerFIO
FROM	dbo.Document D (nolock)
		INNER JOIN dbo.Template T ON T.Id = D.TemplateId
		INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND TT.CodeS = 'AGREEMENT-TEO'
		OUTER APPLY
		(
			SELECT	top 1 RTrim(RTrim(LTrim(C1.F) + ' ' + Coalesce(C1.I, '')) + ' ' + Coalesce(C1.O, '')) AS AgrOwnerFIO, C1.Id AS AgrOwnerId
			FROM	dbo.AgreementOwner AO1 (nolock)
					INNER JOIN dbo.Contact C1 (nolock) ON C1.id = AO1.ContactId
			WHERE	AO1.AgreementId = D.Id
			ORDER BY Coalesce(DateEnd, '21120110') DESC, Coalesce(DateBeg, '19910110')
		) Q1
WHERE	Q1.AgrOwnerFIO IS not NULL
	AND	(
			D.CustomerCompanyId		= @CompanyId
		OR	D.PerformerCompanyId	= @CompanyId
		)
ORDER BY Q1.AgrOwnerFIO
END

GO
