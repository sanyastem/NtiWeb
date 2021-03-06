USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContactInsert]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ContactInsert] (
		@F				nvarchar(MAX),				--Фамилия
		@I				nvarchar(MAX),				--Имя
		@O				nvarchar(MAX),				--Отчество
		@Phone				nvarchar(MAX),			--Телефон
		@Email				nvarchar(MAX),			--Email
		@CompanyId		uniqueidentifier,			--Id компании 
		@CreatedBy		uniqueidentifier			--Кем создан 
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id uniqueidentifier = NEWID(); 
	DECLARE  @CreatedOn datetime = GetDate(); 

	INSERT INTO dbo.Contact(
		Id,
		F,
		I,
		O,
		Phone,
		Email,
		CompanyId,
		CreatedBy,
		CreatedOn
	)
	VALUES (
		@Id,
		@F,
		@I,
		@O,
		@Phone,
		@Email,
		@CompanyId,
		@CreatedBy,
		@CreatedOn
	)
END
	
GO
