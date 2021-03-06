USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserUpdate]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserUpdate]
		@UserId		uniqueidentifier,		
		@ContactId	uniqueidentifier,
		@UserName	nvarchar(256),
		@IsActive	bit,
		@UpdatedBy	uniqueidentifier

AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM dbo.UserList U WHERE (U.Id != @UserId AND U.UserName = @UserName AND IsDeleted = 0))
		BEGIN
			UPDATE UserList

			SET ContactId	= @ContactId,
				UserName	= @UserName,
				IsActive	= @IsActive,
				UpdatedBy	= @UpdatedBy

			WHERE Id = @UserId

			SELECT 'Пользователь успешно изменен'
		END
	ELSE
		SELECT 'Пользователь с таким именем уже существует'
END

GO
