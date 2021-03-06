USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimAgreementGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ClaimAgreementGet](
		@ClaimId		uniqueidentifier,	-- Id заявки
		@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
		@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	AgreementId				= A.Id,																									-- Id договора
			AgreementName			= CASE WHEN @CompanyId = A.CustomerCompanyId THEN A.CustomerName ELSE A.PerformerName END,				-- Наименование договора
			CustomerCompanyId		= A.CustomerCompanyId,																					-- Клиент Id
			CustomerCompanyName		= CC.ShortName,																							-- Клиент
			PerformerCompanyId		= A.PerformerCompanyId,																					-- Экспедитор Id
			PerformerCompanyName	= CP.ShortName																							-- Экспедитор

	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Claim C (nolock) ON C.ClaimId = D.Id
			INNER JOIN dbo.Document2Document D2D (nolock) ON D2D.ChildId = D.Id AND D2D.ParentType = 'Agreement'
			INNER JOIN dbo.Document A (nolock) ON A.Id = D2D.ParentId
			INNER JOIN dbo.Company CC (nolock) ON CC.Id = A.CustomerCompanyId
			INNER JOIN dbo.Company CP (nolock) ON CP.Id = A.PerformerCompanyId

	WHERE	( D.Id = @ClaimId )
END

GO
