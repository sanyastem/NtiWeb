USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserNotificationInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UserNotificationInsert] (
		@UserFromId			uniqueidentifier,			--Id редактируещего пользователя 
		@CompanyFromId		uniqueidentifier,			--Id компании редактирующей 
		@UserToId			uniqueidentifier,			--Id получающего пользователя
		@CompanyToId		uniqueidentifier,			--Id получающей компании 
		@Text				nvarchar(MAX),				--Текст сообщения
		@Url				nvarchar(MAX),				--Метод которому передается управление
		@ObjectId			uniqueidentifier			--Ссылка на измененный объект
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id uniqueidentifier = NEWID(); 
	DECLARE  @CreatedOn datetime = GetDate(); 

	INSERT INTO dbo.UserNotification(
		Id,
		UserFromId,
		CompanyFromId,
		UserToId,
		CompanyToId,
		Text,
		Url,
		ObjectId,
		CreatedBy,
		CreatedOn
	)
	VALUES (
		@Id,
		@UserFromId,
		@CompanyFromId,
		@UserToId,
		@CompanyToId,
		@Text,
		@Url,
		@ObjectId,
		@UserFromId,
		@CreatedOn
	)
	SELECT @Id
END
	

GO
