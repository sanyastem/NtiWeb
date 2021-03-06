USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserListGet]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	UserId				= U.Id,				--Id пользователя	
			FIO					= RTrim(RTrim(LTrim(C.F) + ' ' + Coalesce(C.I, '')) + ' ' + Coalesce(C.O, '')),	
			--F					= C.F,				--Фамилия
			--I					= C.I,				--Имя
			--O					= C.O,				--Отчество
			Login				= U.UserName,		--Логин
			IsDeleted			= U.IsDeleted,
			IsActive			= U.IsActive 
	FROM	dbo.UserList U (nolock)
			LEFT JOIN dbo.Contact C (nolock) ON U.ContactId = C.Id
	ORDER BY FIO
	--WHERE U.IsDeleted = 0 --AND U.IsActive = 1 
END

GO
