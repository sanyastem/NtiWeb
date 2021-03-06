USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ReportPeriodListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportPeriodListGet]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	RP.Id, RP.Name
	FROM	NSI.ReportPeriod RP (nolock)
			INNER JOIN NSI.Reference R (nolock) ON R.Id = RP.TypeId
	WHERE	RP.IsDeleted = 0 AND R.CodeS = 'M' AND RP.DateBeg < DateAdd(month, 2, GetDate())
	ORDER BY RP.DateBeg DESC
END

GO
