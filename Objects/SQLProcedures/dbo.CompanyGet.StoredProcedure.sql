USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[CompanyGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CompanyGet]
	@CompanyId				uniqueidentifier = NULL
AS
BEGIN
	SELECT 
		c.Id,
		c.FullName,
		c.ShortName,
		c.ViewName,
		c.Note,

		CodeOKPO = O2CV_OKPO.ValueString,
		CodeINN = O2CV_INN.ValueString
FROM dbo.Company C (nolock)
  LEFT JOIN dbo.vObject2ConditionValue O2CV_OKPO (nolock) ON O2CV_OKPO.ObjectId = C.Id AND O2CV_OKPO.TableName = 'dbo.Company' AND O2CV_OKPO.MnemoCode = 'CodeOKPO'
--  LEFT JOIN dbo.ConditionValue CV_OKPO (nolock) ON CV_OKPO.Id = O2CV_OKPO.ConditionValueId
---  LEFT JOIN dbo.Condition C_OKPO (nolock) ON C_OKPO.Id = CV_OKPO.ConditionId AND C_OKPO.MnemoCode = 'CodeOKPO'
  LEFT JOIN dbo.vObject2ConditionValue O2CV_INN (nolock) ON O2CV_INN.ObjectId = C.Id AND O2CV_INN.TableName = 'dbo.Company' AND O2CV_INN.MnemoCode = 'CodeINN'
--  LEFT JOIN dbo.ConditionValue CV_INN (nolock) ON CV_INN.Id = O2CV_INN.ConditionValueId
--  LEFT JOIN dbo.Condition C_INN (nolock) ON C_INN.Id = CV_INN.ConditionId AND C_INN.MnemoCode = 'CodeINN'
WHERE (@CompanyId IS NULL OR C.Id = @CompanyId)
END

GO
