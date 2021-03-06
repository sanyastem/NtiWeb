USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserInsert] (
		@ContactId			uniqueidentifier,
		@UserName			nvarchar(256),
		@PasswordHash		nvarchar(MAX),
		@SecurityStamp		nvarchar(MAX),
		@CreatedBy			uniqueidentifier
)
		
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewUserId uniqueidentifier = NEWID();

	IF NOT EXISTS (SELECT 1 FROM dbo.UserList U WHERE (U.UserName = @UserName AND U.IsDeleted = 0))

		BEGIN
			INSERT INTO dbo.UserList (
				Id,
				IsActive,
				UserName,
				PasswordHash,
				SecurityStamp,
				ContactId,
				CreatedBy,
				CreatedOn,
				IsDeleted
			)

			VALUES (
				@NewUserId,
				1,
				@UserName,
				@PasswordHash,
				@SecurityStamp,
				@ContactId,
				@CreatedBy,
				GetDate(),
				0
			)

			SELECT CONVERT(nvarchar(50), @NewUserId)
		END
	ELSE 
		SELECT 'Пользователь с таким имененм уже существует'

END

GO
