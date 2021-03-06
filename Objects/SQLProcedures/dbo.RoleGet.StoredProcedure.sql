USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[RoleGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleGet]
	@RoleId				uniqueidentifier = null
AS
BEGIN
	SELECT
		r.*
	FROM
		dbo.Role r
	WHERE
		(@RoleId IS NULL OR r.Id = @RoleId)
		AND (r.IsDeleted = 0)
END

GO
