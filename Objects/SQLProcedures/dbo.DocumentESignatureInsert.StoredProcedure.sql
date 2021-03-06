USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentESignatureInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DocumentESignatureInsert] (
		@AttachId			uniqueidentifier,			--Id записи с документом в таблице DocumentAttach 
		@CompanyId			uniqueidentifier,			--Id компании, которая подписыват
		@Content			nvarchar(MAX),					--Файл подписи
		@CreatedBy			uniqueidentifier			--Id подписанта
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id uniqueidentifier = NEWID(); 
	DECLARE  @CreatedOn datetime = GetDate(); 

	INSERT INTO dbo.DocumentESignature(
		Id,
		AttachId,
		CreatedBy,
		CreatedOn, 
		Сontent,
		CompanyId
	)
	VALUES (
		@Id,
		@AttachId,
		@CreatedBy,
		@CreatedOn,
		@Content,
		@CompanyId
	)
END
	
GO
