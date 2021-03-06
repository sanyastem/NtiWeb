USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimLoadingScheduleGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ClaimLoadingScheduleGet](
				@ClaimId		uniqueidentifier,														--Id заявки '21DB81C7-C949-4E34-9CE5-004AD6EB492E'
				@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
				@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	Version		= CS.Version,
			StatusName	= R.Name,
			LoadDate	= CSD.LoadDate,
			FrWeight	= CSD.FrWeight,
			CarCount	= CSD.CarCount

	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Claim C (nolock) ON C.ClaimId = D.Id
			LEFT JOIN dbo.ClaimLoadingSchedule CS (nolock) ON CS.ClaimId = @ClaimId 
			LEFT JOIN NSI.Reference R (nolock) ON R.Id = CS.StatusId AND R.RefTypeId = 'be211682-4251-4ac6-8377-a2aae8326dce' AND CodeS = 'LS'
			LEFT JOIN dbo.ClaimLoadingScheduleDate CSD (nolock) ON CSD.LoadingScheduleId = CS.Id 

	WHERE	( D.Id = @ClaimId )
END

GO
