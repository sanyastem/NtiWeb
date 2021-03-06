USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContextHelpInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ContextHelpInsert] (
		@PortalUrl				nvarchar(MAX),			-- Url портала
		@HelpUrl				nvarchar(MAX)			-- Url контекстной справки
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id uniqueidentifier = NEWID(); 

	INSERT INTO dbo.ContextHelp(
		Id,
		PortalUrl,
		HelpUrl		
	)
	VALUES (
		@Id,
		@PortalUrl,
		@HelpUrl
	)
END
	

GO
