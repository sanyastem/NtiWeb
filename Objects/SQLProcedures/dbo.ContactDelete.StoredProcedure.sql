USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ContactDelete]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ContactDelete] (
		@Id				uniqueidentifier			--Id контакта
		)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE  @UpdatedOn datetime = GetDate(); 

	UPDATE dbo.Contact Set
			IsDeleted = 1
	WHERE Id = @Id
END
	
GO
