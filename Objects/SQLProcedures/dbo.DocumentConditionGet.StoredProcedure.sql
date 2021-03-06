USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[DocumentConditionGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DocumentConditionGet]
	@DocumentId		uniqueidentifier,
	@Date			date				= NULL				-- Дата на каторую отображаются параметры договора
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	G.Name AS GroupName, O2CV.PositionGroup, O2CV.PositionValue, O2CV.Name, O2CV.ValueString, O2CV.ConditionLimitValue
	FROM	dbo.vObject2ConditionValue O2CV
			INNER JOIN NSI.Reference G (nolock) ON G.Id = O2CV.ConditionGroupId
	WHERE	O2CV.TableName	= 'dbo.Document'
		AND	O2CV.ObjectId	= @DocumentId
	ORDER BY O2CV.PositionGroup, O2CV.PositionValue
END

GO
