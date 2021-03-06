USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserDetailsListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UserDetailsListGet](
		@UserId uniqueidentifier)					--Id пользователя
AS

BEGIN
	SET NOCOUNT ON;

	SELECT	CompanyId		= C.Id,					--Id компании
			CompanyName		= C.ShortName,			--Название компании
			CompanyNote		= C.Note,				--Примечание

			CodeOKPO		= CV_OKPO.ValueString,	-- ОКПО
			CodeINN			= CV_INN.ValueString	-- ИНН

	FROM Employee E
		INNER JOIN Company C ON E.CompanyId = C.Id
		INNER JOIN dbo.Object2ConditionValue O2CV_OKPO (nolock) ON O2CV_OKPO.ObjectId = C.Id AND O2CV_OKPO.TableName = 'dbo.Company'
		INNER JOIN dbo.ConditionValue CV_OKPO (nolock) ON CV_OKPO.Id = O2CV_OKPO.ConditionValueId
		INNER JOIN dbo.Condition C_OKPO (nolock) ON C_OKPO.Id = CV_OKPO.ConditionId AND C_OKPO.MnemoCode = 'CodeOKPO'
		INNER JOIN dbo.Object2ConditionValue O2CV_INN (nolock) ON O2CV_INN.ObjectId = C.Id AND O2CV_INN.TableName = 'dbo.Company'
		INNER JOIN dbo.ConditionValue CV_INN (nolock) ON CV_INN.Id = O2CV_INN.ConditionValueId
		INNER JOIN dbo.Condition C_INN (nolock) ON C_INN.Id = CV_INN.ConditionId AND C_INN.MnemoCode = 'CodeINN'

	WHERE E.UserId = @UserId AND E.IsDeleted = 0
END

GO
