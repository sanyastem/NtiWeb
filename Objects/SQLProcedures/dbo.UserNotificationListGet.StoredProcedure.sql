USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserNotificationListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UserNotificationListGet] (
	@NotificationId uniqueidentifier,		--Id уведомления
	@UserToId		uniqueidentifier		--Id получателя
)		
AS
BEGIN

	SET NOCOUNT ON;

	
	SELECT 
		Id			= N.Id,
		UserFromId = N.UserFromId,
		UserToID = N.UserToId,
		UserFromName	= RTrim(RTrim(LTrim(CC.F) + ' ' + Coalesce(CC.I, '')) + ' ' + Coalesce(CC.O, '')),
		UserToName = RTrim(RTrim(LTrim(UC.F) + ' ' + Coalesce(UC.I, '')) + ' ' + Coalesce(UC.O, '')),
		CompanyFromId = N.CompanyFromId,
		CompanyToId = N.CompanyToId,
		CompanyFromName = C1.FullName,
		CompanyToName =  C2.FullName,
		Text = N.Text,
		Url = N.Url,
		CreatedOn = N.CreatedOn,
		ViewDate = N.ViewDate,
		ObjectId = N.ObjectId

	FROM dbo.UserNotification N		
		 LEFT JOIN dbo.UserList U (nolock) ON N.UserFromId = U.Id			--пользователь, спровоцировавший создание сообщения
		 LEFT JOIN dbo.UserList U2 (nolock) ON N.UserToId = U2.Id			--пользователь, которому предназначено сообщение
		 LEFT JOIN dbo.Company C1 (nolock) ON N.CompanyFromId = C1.Id
		 LEFT JOIN dbo.Company C2 (nolock) ON N.CompanyToId = C2.Id
		 LEFT JOIN dbo.Contact CC (nolock) ON U.ContactId = CC.Id
		 LEFT JOIN dbo.Contact UC (nolock) ON U2.ContactId = UC.Id

 	WHERE 
		(@NotificationId IS NULL OR N.Id = @NotificationId) AND (UserToID = @UserToId) AND (N.IsDeleted = 0)
	ORDER BY CreatedOn DESC
END

GO
