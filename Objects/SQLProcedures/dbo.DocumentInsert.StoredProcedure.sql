USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DocumentInsert]
	@DocNumber nvarchar(50),
	@CustomerRegNumber nvarchar(50),
	@PerformerRegNumber nvarchar(50),
	@DocDate date,
	@CustomerName nvarchar(256),
	@PerformerName nvarchar(256),
	@CustomerCompanyId uniqueidentifier,
	@CustomerContactId uniqueidentifier,
	@PerformerCompanyId uniqueidentifier,
	@PerformerContactId uniqueidentifier,
	@StatusId uniqueidentifier,
	@TemplateId uniqueidentifier,
	@Note nvarchar(max),
	@CreatedBy uniqueidentifier
AS
	DECLARE @op TABLE (
		Id uniqueidentifier
	)
BEGIN
	INSERT INTO dbo.Document (
		DocNumber,
		CustomerRegNumber,
		PerformerRegNumber,
		DocDate,
		CustomerName,
		PerformerName,
		CustomerCompanyId,
		CustomerContactId,
		PerformerCompanyId,
		PerformerContactId,
		StatusId,
		TemplateId,
		Note,
		CreatedBy)
	OUTPUT inserted.Id INTO @op
	VALUES (
		@DocNumber,
		@CustomerRegNumber,
		@PerformerRegNumber,
		@DocDate,
		@CustomerName,
		@PerformerName,
		@CustomerCompanyId,
		@CustomerContactId,
		@PerformerCompanyId,
		@PerformerContactId,
		@StatusId,
		@TemplateId,
		@Note,
		@CreatedBy)

	SELECT Id FROM @op
END

GO
