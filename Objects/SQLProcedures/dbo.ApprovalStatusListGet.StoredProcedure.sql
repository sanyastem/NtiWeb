USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ApprovalStatusListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ApprovalStatusListGet]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	RC.Id, RC.Name
	FROM	NSI.Reference RC (nolock)
			INNER JOIN NSI.Reference RP (nolock) ON RP.Id = RC.ParentReferenceId
	WHERE	RC.IsDeleted = 0 AND RP.CodeS = 'CLAIM-TEO-CLI'
END

GO
