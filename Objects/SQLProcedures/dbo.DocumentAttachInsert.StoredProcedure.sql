USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentAttachInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DocumentAttachInsert] (
		@DocumentId			uniqueidentifier,			--Id 
		@FileName			nvarchar(MAX),				--Имя файла 
		@Content			varbinary(MAX),					--Файл
		@CreatedBy			uniqueidentifier			--Id создателя
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id uniqueidentifier = NEWID(); 
	DECLARE  @CreatedOn datetime = GetDate(); 

	INSERT INTO dbo.DocumentAttach(
		Id,
		DocumentId,
		FName,
		CreatedBy,
		CreatedOn, 
		AttachContent
	)
	VALUES (
		@Id,
		@DocumentId,
		@FileName,
		@CreatedBy,
		@CreatedOn,
		@Content
	)
	SELECT @Id
END
	

GO
