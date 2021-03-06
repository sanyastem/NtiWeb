USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[RoleInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleInsert] (
		@Name				nvarchar(MAX),
		@CreatedBy			uniqueidentifier
)
		
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewUserId uniqueidentifier = NEWID();
	IF NOT EXISTS (SELECT 1 FROM dbo.Role R WHERE (R.Name = @Name AND R.IsDeleted = 0))

		BEGIN
			INSERT INTO dbo.Role(
				Id,
				Name,
				CreatedBy,
				CreatedOn
			)

			VALUES (
				@NewUserId,
				@Name,
				@CreatedBy,
				GetDate()
			)

			SELECT CONVERT(nvarchar(50), @NewUserId)
		END
	ELSE 
		SELECT 'Роль с таким имененм уже существует'

END

GO
