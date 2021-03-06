USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimStateGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ClaimStateGet] (
	@ClaimId		uniqueidentifier,	-- Id заявки
	@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
	@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
)
AS
BEGIN
	DECLARE @Cap uniqueidentifier = NEWID();

	SET NOCOUNT ON;

	SELECT	ApprovalClaimName				= '',										-- заглушка Согласование заявки
			ApprovalClaimGraphName			= '',										-- заглушка Согласование графика погрузки
			ProtocolId						= @Cap,										-- заглушка Протокол Id
			ProtocolName					= '',										-- заглушка Протокол
			AccountId						= @Cap,										-- заглушка Счет на предоплату Id
			AccountName						= '',										-- заглушка Счет на предоплату			
			
			ProtocolApprovalPerformerImgId  = @Cap,
			ProtocolApprovalCustomerImgId	= @Cap,
			ProtocolSigningPerformerImgId	= @Cap,
			ProtocolSigningCustomerImgId	= @Cap,

			ClaimApprovalSigner1ImgId		= @Cap,
			ClaimApprovalSigner2ImgId		= @Cap,
			ClaimSigningSigner1ImgId		= @Cap,
			ClaimSigningSigner2ImgId		= @Cap	
END

GO
