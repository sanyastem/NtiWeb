USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[RoleUpdate]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleUpdate]
		@RoleId			uniqueidentifier,	
		@Name		nvarchar(MAX),	
		@UpdatedBy		uniqueidentifier

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Role SET 
	Name = @Name,
	UpdatedOn = GetDate(),
	UpdatedBy = @UpdatedBy

	WHERE Id = @RoleId
END

GO
