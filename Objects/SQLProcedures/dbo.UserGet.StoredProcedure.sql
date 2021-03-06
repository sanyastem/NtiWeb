USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserGet]
	@UserId				uniqueidentifier = NULL
AS
BEGIN
	SELECT 
		u.*
	FROM 
		dbo.UserList u
	WHERE
		(@UserId IS NULL AND u.IsDeleted = 0) OR (u.Id = @UserId)
END

GO
