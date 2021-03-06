USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[SupplementaryAgreementListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SupplementaryAgreementListGet]
	@AgreementId	uniqueidentifier,
	@OnlyActive		bit					= 1
AS
BEGIN
	SET NOCOUNT ON;

	IF @OnlyActive IS NULL SET @OnlyActive = 1

	SELECT	Id				= D.Id,
			DocNumber		= D.DocNumber,
			DocDate			= D.DocDate,
			DocName			= D.CustomerName,
			DateBeg			= A.DateBeg,
			DateEnd			= Coalesce(A.DateStop, A.DateEnd),
			TemplateName	= T.Name,
			DocStatus		= CASE WHEN GetDate() between Coalesce(A.DateBeg, '19910101') AND Coalesce(A.DateStop, A.DateEnd, '20500101') THEN 1 ELSE 0 END
	FROM	dbo.Document2Document D2C (nolock)
			INNER JOIN dbo.Document D (nolock) ON D.Id = D2C.ChildId
			INNER JOIN dbo.Agreement A (nolock) ON A.DocumentId = D.Id
			INNER JOIN dbo.Template T (nolock) ON T.Id = D.TemplateId
	WHERE	D2C.ParentId = @AgreementId
		AND	(
				( @OnlyActive = 0 )
			OR	( @OnlyActive = 1 AND GetDate() between Coalesce(A.DateBeg, '19910101') AND Coalesce(A.DateStop, A.DateEnd, '20500101') )
			)
END

GO
