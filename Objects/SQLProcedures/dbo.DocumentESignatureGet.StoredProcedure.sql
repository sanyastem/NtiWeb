USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentESignatureGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DocumentESignatureGet]
	@DocumentId				uniqueidentifier	= NULL			--Id документа
AS
BEGIN
	SELECT 	
		AttachId = DS.AttachId, 
		Content = DS.Сontent,
		CompanyId = DS.CompanyId,
		CreatedBy = DS.CreatedBy,
		CreatedOn = DS.CreatedOn
	FROM dbo.DocumentESignature DS
			INNER JOIN dbo.DocumentAttach DA (nolock) ON DA.DocumentId = @DocumentId AND (DA.IsDeleted = 0)
	WHERE DS.AttachId = DA.Id AND DS.IsDeleted = 0
END

GO
