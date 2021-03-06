USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[PermissionInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PermissionInsert]
	@RusName			nvarchar(256),
	@EngName			nvarchar(256),
	@CreatedBy			uniqueidentifier
AS
	
BEGIN
	INSERT INTO dbo.Permission
	(Code, Name, Code2, Module, CreatedBy)
	VALUES
		(@EngName+':C',
		@RusName + ', ' + @RusName + ' >> ' + 'Добавление',
		@EngName+'Create',
		@RusName,
		@CreatedBy);

	INSERT INTO dbo.Permission
		(Code, Name, Code2, Module, CreatedBy)
		VALUES
			(@EngName+':E',
			@RusName + ', ' + @RusName + ' >> ' + 'Редактирование',
			@EngName+'Edit',
			@RusName,
			@CreatedBy);

	INSERT INTO dbo.Permission
	(Code, Name, Code2, Module, CreatedBy)
	VALUES
		(@EngName+':D',
		@RusName + ', ' + @RusName + ' >> ' + 'Удаление',
		@EngName+'Delete',
		@RusName,
		@CreatedBy);

	INSERT INTO dbo.Permission
	(Code, Name, Code2, Module, CreatedBy)
	VALUES
		(@EngName+':V',
		@RusName + ', ' + @RusName + ' >> ' + 'Просмотр',
		@EngName+'View',
		@RusName,
		@CreatedBy);
END

GO
