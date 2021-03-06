USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentAttachGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DocumentAttachGet]
--	@DocumentId uniqueidentifier	= NULL
	@DocumentId uniqueidentifier
AS
BEGIN
	SELECT 	
		Id = D.Id,
		DocumentId = D.DocumentId, 
		Content = D.AttachContent,
		FName = D.FName, 
		CreatedBy = D.CreatedBy,
		CreatedOn = D.CreatedOn
	
	FROM dbo.DocumentAttach D
--	WHERE (@DocumentId IS NULL OR D.DocumentId = @DocumentId) AND D.IsDeleted = 0
	WHERE	D.IsDeleted = 0 AND D.DocumentId = @DocumentId
END

GO
