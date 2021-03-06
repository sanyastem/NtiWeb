USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[PermissionListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PermissionListGet](
		@RoleId			uniqueidentifier					--Id роли
		)
AS

BEGIN
	SET NOCOUNT ON;
	IF @RoleId IS NOT NULL
		BEGIN
			SELECT	Id				=P.Id,
					Name			=P.Name,
					Code			=P.Code,
					Code2			=P.Code2,
					Module			=P.Module
			FROM Role R
				LEFT JOIN dbo.RolePermission RP (nolock) ON (R.Id = RP.RoleId AND RP.IsDeleted=0)
				LEFT JOIN dbo.Permission P (nolock) ON  RP.PermissionId = P.Id 
			WHERE R.IsDeleted = 0 AND  RP.IsDeleted =0 AND (R.Id = @RoleId OR @RoleId IS NULL) 
		END
	ELSE
		SELECT	Id				=P.Id,
				Name			=P.Name,
				Code			=P.Code,
				Code2			=P.Code2,
				Module			=P.Module

		FROM Permission P
		WHERE P.IsDeleted = 0 AND P.IsDeleted=0

END

GO
