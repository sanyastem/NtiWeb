USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementUpdate]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgreementUpdate]
	@DocumentId				uniqueidentifier,
	@DateBeg				date,
	@DateEnd				date,
	@DateStop				date,
	@СustomerBankId			uniqueidentifier,
	@CustomerAddressId		uniqueidentifier,
	@PerformerBankId		uniqueidentifier,
	@PerformerAddressId		uniqueidentifier,
	@UpdatedBy				uniqueidentifier
AS
BEGIN
	UPDATE
		dbo.Agreement
	SET
		DateBeg = @DateBeg,
		DateEnd = @DateEnd,
		DateStop = @DateStop,
		СustomerBankId = @СustomerBankId,
		СustomerAddressId = @CustomerAddressId,
		PerformerBankId = @PerformerBankId,
		PerformerAddressId = @PerformerAddressId,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = GETDATE()
	WHERE
		DocumentId = @DocumentId
END

GO
