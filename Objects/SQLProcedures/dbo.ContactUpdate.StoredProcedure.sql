USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContactUpdate]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ContactUpdate] (
		@Id				uniqueidentifier,			--Id контакта
		@F				nvarchar(MAX),				--Фамилия
		@I				nvarchar(MAX),				--Имя
		@O				nvarchar(MAX),				--Отчество
		@Phone			nvarchar(MAX),				--Телефон
		@Email			nvarchar(MAX),				--Email
		@CompanyId		uniqueidentifier,			--Id компании 
		@UpdatedBy		uniqueidentifier			--Кем создан 
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE  @UpdatedOn datetime = GetDate(); 

	UPDATE dbo.Contact Set
		F = @F,
		I = @I,
		O = @O,
		Phone = @Phone,
		Email = @Email,
		CompanyId = @CompanyId,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = @UpdatedOn
	WHERE Id = @Id
END
	
GO
