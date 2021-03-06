USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContextHelpGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ContextHelpGet]
	@PortalUrl				nvarchar(MAX) = NULL
AS
BEGIN
	SELECT 
		c.Id,
		c.PortalUrl,
		c.HelpUrl

FROM dbo.ContextHelp C (nolock)
WHERE (@PortalUrl IS NULL OR C.PortalUrl = @PortalUrl)
END

GO
