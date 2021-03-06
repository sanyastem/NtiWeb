USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgreementGet]
	@CompanyId		uniqueidentifier,										-- Организация, по отношению к которой выбирается данные Договора
	@Id				uniqueidentifier										-- Идентификатор Договора
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	DocumentId				= doc.Id,																										-- Id Договора
			TemplateId				= doc.TemplateId,																								-- Id шаблона Договора
			TemplateName			= tpl.Name,																										-- Наименование шаблона Договора
			DocNumber				= doc.DocNumber,																								-- Номер документа
			RegNumber				= CASE WHEN @CompanyId = doc.CustomerCompanyId THEN doc.CustomerRegNumber ELSE doc.PerformerRegNumber END,		-- Регистрационный номер Договора
			DocDate					= doc.DocDate,																									-- Дата договора
			StatusId				= doc.StatusId,																									-- Id статуса Договора
			StatusName				= '',																											-- Наименование статуса Договора
			DocName					= CASE WHEN @CompanyId = doc.CustomerCompanyId THEN doc.CustomerName ELSE doc.PerformerName END,				-- Наименование Договора
			CustomerCompanyId		= doc.CustomerCompanyId,																						-- Id компании Заказчика
			CustomerCompanyName		= ccmp.ShortName + CASE WHEN ccmp.AddName <> '' THEN ' (' + ccmp.AddName + ')' ELSE '' END,						-- Наименование компании Заказчика
			CustomerCompanyLabel	= Coalesce(cocv.ValueString, 'Заказчик'),																		-- Заголовок компании Заказчика
			PerformerCompanyId		= doc.PerformerCompanyId,																						-- Id компании Исполнителя
			PerformerCompanyName	= pcmp.ShortName + CASE WHEN pcmp.AddName <> '' THEN ' (' + pcmp.AddName + ')' ELSE '' END,						-- Наименование компании Исполнителя
			PerformerCompanyLabel	= Coalesce(pocv.ValueString, 'Исполнитель'),																	-- Заголовок компании Исполнителя
			СustomerBankId			= agr.СustomerBankId,																							-- Id банка Заказчика
			CustomerBankName		= '',																											-- Наименование банка Заказчика
			PerformerBankId			= agr.PerformerBankId,																							-- Id банка Исполнителя
			PerformerBankName		= '',																											-- Наименование банка Исполнителя
			СustomerAddressId		= agr.СustomerAddressId,																						-- Id адреса Заказчика
			CustomerAddressName		= '',																											-- Наименование адреса Заказчика
			PerformerAddressId		= agr.PerformerAddressId,																						-- Id адреса Исполнителя
			PerformerAddressName	= '',																											-- Наименование адреса Исполнителя
			CustomerSignerId		= doc.CustomerContactId,																						-- Id подписанта со стороны Заказчика
			CustomerSignerName		= '',																											-- Наименование подписанта со стороны Заказчика
			PerformerSignerId		= doc.PerformerContactId,																						-- Id подписанта со стороны Исполнителя
			PerformerSignerName		= '',																											-- Наименование подписанта со стороны Исполнителя
			DateBeg					= agr.DateBeg,																									-- Дата начала действия Договора
			DateEnd					= agr.DateEnd,																									-- Дата окончания действия Договора
			DateStop				= agr.DateStop,																									-- Дата приостановки действия Договора
			CustomerManagerId		= NULL,																											-- Id менеджера ответственный по договору со стороны Заказчика
			CustomerManagerName		= '',																											-- Имя менеджера ответственный по договору со стороны Заказчика
			PerformerManagerId		= NULL,																											-- Id менеджера ответственный по договору со стороны Исполнителя
			PerformerManagerName	= '',																											-- Имя менеджера ответственный по договору со стороны Исполнителя
			Note					= doc.Note,																										-- Примечание к Договору
			CreatedBy				= doc.CreatedBy,																								-- Кем создан Договора (Id)
			CreatedByName			= cusr.ContactName,																								-- Кем создан Договора (Имя)
			CreatedOn				= doc.CreatedOn,																								-- Дата создания Договора
			UpdatedBy				= doc.UpdatedBy,																								-- Кем Договор изменялся в последний раз (Id)
			UpdatedByName			= uusr.ContactName,																								-- Кем Договор изменялся в последний раз (Имя)
			UpdatedOn				= doc.UpdatedOn,																								-- Дата последнего изменения Договора
			IRSPerevozkiId			= irs.ValueInteger																								-- Id Договора в системе ИРС Перевозки

	FROM	dbo.Document doc (nolock)
			INNER JOIN dbo.Agreement agr (nolock) ON agr.DocumentId = doc.Id
			INNER JOIN dbo.Template tpl (nolock) ON tpl.Id = doc.TemplateId
			LEFT JOIN dbo.vCompany ccmp ON ccmp.Id = doc.CustomerCompanyId
			LEFT JOIN dbo.vObject2ConditionValue cocv ON cocv.TableName = 'dbo.Document' AND cocv.ObjectId = doc.Id AND cocv.MnemoCode = 'CustomerLabelName'
			LEFT JOIN dbo.vCompany pcmp ON pcmp.Id = doc.PerformerCompanyId
			LEFT JOIN dbo.vObject2ConditionValue pocv ON pocv.TableName = 'dbo.Document' AND pocv.ObjectId = doc.Id AND pocv.MnemoCode = 'PerformerLabelName'
			INNER JOIN dbo.vUserList cusr on cusr.Id = doc.CreatedBy
			LEFT JOIN dbo.vUserList uusr on uusr.Id = doc.UpdatedBy
			LEFT JOIN dbo.vObject2ConditionValue irs ON irs.TableName = 'dbo.Document' AND irs.ObjectId = doc.Id AND irs.MnemoCode = 'IRSPerevozkiId_RAT'
	WHERE	doc.Id = @Id
/*---------------------------------------------------------------------------------------------------------------------------------------------------
 Version      Date    By    Changes  
 -------   --------   ---   -------------------------------------------------------------------------------------------------------------------------
 1.0       05/03/17   brv   Created
 -------   --------   ---   -------------------------------------------------------------------------------------------------------------------------
 P U T   Y O U R   C H A N G E   H I S T O R Y   A T    T H E   T O P !
---------------------------------------------------------------------------------------------------------------------------------------------------*/
END

GO
