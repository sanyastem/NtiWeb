USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AccountGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AccountGet]
	@AccountId				uniqueidentifier 
AS
BEGIN
	SELECT 
		Id								= @AccountId,
		TemplateName					= 'Счет на оплату перевозки',											--Шаблон
		DocNumber						= '8400',																--Номер, nvarchar
		DocDate							= '01-01-2020',															--Дата
		DocStatusId						= '',																	--Id статуса документа
		DocStatusName					= '',																	--Статус документа																	
		DocName							= 'Протокол по единой ставке № РАТ/ЦО/13-ТУ-410 от 02.01.2017',			--наименование документа																	--Наименование
		AgreementId						= CAST ('c66a0136-6515-4e1e-9c53-ed6fb06dc4b5' as uniqueidentifier),	--Id договора
		AgreementName					= 'РАТ/ЦО/13-ТУ-501//АО "Рязаньзернопродукт"',							--Договор
		CustomerCompanyId				= CAST ('c66a0136-6515-4e1e-9c53-ed6fb06dc4b5' as uniqueidentifier),	--Id компании клиента	
		CustomerCompanyName				= 'ООО "ТСС-М"',														--Наименование компании клиента		
		PerformerCompanyId				= CAST ('c66a0136-6515-4e1e-9c53-ed6fb06dc4b5' as uniqueidentifier),	--Id экспедитора																	--Id экспедитора																													
		PerformerCompanyName 			= 'Сидоров Иван Иванович',												--Экспедитор																																
		ClaimId							= CAST ('c66a0136-6515-4e1e-9c53-ed6fb06dc4b5' as uniqueidentifier),	--Id заявки
		ClaimName						= 'Заявка клиента № 465 от 30.09.2016',									--Заявка
		RepoptPeriodName				= '2017-Сентябрь',														--Отчетный период		
		DateBeg							= '01-01-1990',															--Дата начала отчетного периода
		DateEnd							= '31-12-2050',															--Дата окончания отчетного периода
		SummaWithNDS					= '123123123,00',														--Сумма счета с НДС
		CurrencyName					= 'Белорусский рубль',													--Наименование валюты			
		SummaNDS						= '123123123,00',														--"В том числе с НДС"
		NULL AS CreatedBy,
		NULL AS CreatedOn,
		NULL AS UpdatedBy,
		NULL AS UpdatedOn,
		NULL AS IsDeleted,
			IRSPerevozkiId			= irs.ValueInteger		
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Template T (nolock) ON T.Id = D.TemplateId
			LEFT JOIN dbo.vObject2ConditionValue irs ON irs.TableName = 'dbo.Document' AND irs.ObjectId = D.Id AND irs.MnemoCode = 'IRSPerevozkiId_RAT'
	WHERE	D.Id = @AccountId
END

GO
