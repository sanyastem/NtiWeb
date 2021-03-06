USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgreementInsert]
	@DocumentId				uniqueidentifier,
	@DateBeg				date,
	@DateEnd				date,
	@DateStop				date,
	@СustomerBankId			uniqueidentifier,
	@СustomerAddressId		uniqueidentifier,
	@PerformerBankId		uniqueidentifier,
	@PerformerAddressId		uniqueidentifier,
	@CreatedBy				uniqueidentifier
AS
BEGIN
	INSERT INTO dbo.Agreement (
		DocumentId,
		DateBeg,
		DateEnd,
		DateStop,
		СustomerBankId,
		СustomerAddressId,
		PerformerBankId,
		PerformerAddressId,
		CreatedBy)
	VALUES (
		@DocumentId,
		@DateBeg,
		@DateEnd,
		@DateStop,
		@СustomerBankId,
		@СustomerAddressId,
		@PerformerBankId,
		@PerformerAddressId,
		@CreatedBy)
END

GO
