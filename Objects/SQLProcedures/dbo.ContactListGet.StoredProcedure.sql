USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContactListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ContactListGet]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	Id	= C.Id,						--Id контакта 
			F	= C.F,						--Фамилия
			I	= C.I,						--Имя
			O	= C.O,						--Отчество
			Email = C.Email,				--Email
			Phone = C.Phone,				--Телефонный номер
			Company = F.ShortName			--Наименование компании
				
	FROM	dbo.Contact C (nolock)
			INNER JOIN dbo.Company F (nolock) ON F.Id = C.CompanyId

	WHERE  C.IsDeleted = 0
	ORDER BY F
END

GO
