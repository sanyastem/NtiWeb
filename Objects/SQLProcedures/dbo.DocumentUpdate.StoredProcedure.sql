USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentUpdate]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DocumentUpdate]
--@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',	-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@DocumentId				uniqueidentifier,
	@DocNumber				nvarchar(50),
--@AgreementRegNumber		nvarchar(50),
	@CustomerRegNumber		nvarchar(50),		--Лишнее
	@PerformerRegNumber		nvarchar(50),		--Лишнее
	@DocDate				date,
--@AgreementName	nvarchar(50),
	@CustomerName			nvarchar(256),		--Лишнее
	@PerformerName			nvarchar(256),		--Лишнее
	@CustomerCompanyId		uniqueidentifier,
	@CustomerContactId		uniqueidentifier,
	@PerformerCompanyId		uniqueidentifier,
	@PerformerContactId		uniqueidentifier,
	@StatusId				uniqueidentifier,
	@TemplateId				uniqueidentifier,
	@Note					nvarchar(max),
	@UpdatedBy				uniqueidentifier
AS
BEGIN
	UPDATE 
		dbo.Document 
	SET
		DocNumber = @DocNumber,
		CustomerRegNumber = @CustomerRegNumber, --CASE WHEN CustomerRegNumber IS NULL OR CustomerCompanyId = @CompanyId THEN @AgreementRegNumber ELSE CustomerRegNumber END,
		PerformerRegNumber = @PerformerRegNumber,
		DocDate = @DocDate,
--		CustomerName = @CustomerName,
--		PerformerName = @PerformerName,
		CustomerCompanyId = @CustomerCompanyId,
		CustomerContactId = @CustomerContactId,
		PerformerCompanyId = @PerformerCompanyId,
		PerformerContactId = @PerformerContactId,
		StatusId = @StatusId,
		TemplateId = @TemplateId,
		Note = @Note,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = GETDATE()
	WHERE 
		(Id = @DocumentId)
END

GO
