USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementDetele]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgreementDetele]
	@DocumentId				uniqueidentifier
AS
BEGIN
	UPDATE dbo.AgreementOwner SET IsDeleted = 1 
	WHERE AgreementId = @DocumentId;
	
	UPDATE dbo.Agreement SET IsDeleted = 1 
	WHERE DocumentId = @DocumentId;

    UPDATE dbo.Document2Document SET IsDeleted = 1 
	WHERE ParentId = @DocumentId;

	--UPDATE dbo.Document2Document SET IsDeleted = 1 
	--WHERE ChildId = @DocumentId;

	UPDATE dbo.DocumentAttach SET IsDeleted = 1
	WHERE DocumentId = @DocumentId;
	
	UPDATE dbo.Document SET IsDeleted = 1 
	WHERE Id = @DocumentId;
END

GO
