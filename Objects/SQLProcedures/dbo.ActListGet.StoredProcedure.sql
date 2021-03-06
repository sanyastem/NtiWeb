USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ActListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActListGet]
	@CompanyId		uniqueidentifier,												-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256),													-- Группа (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'ACT%')
	@DocPeriodType	integer				= 5,										-- Тип периода создания документа (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@DocDateBeg		date				= NULL,										-- Период С
	@DocDateEnd		date				= NULL,										-- Период ПО										
	@RepPeriodType	integer				= 2,										-- Тип отчетного периода (0-Все, 1-За период С ПО, 2-За предыдущий месяц, 3-За предыдущий квартал, 4-За предыдущий год)
	@RepDateBeg		date				= NULL,										-- Период С
	@RepDateEnd		date				= NULL,										-- Период ПО										
	@AgreementId	uniqueidentifier	= NULL,										-- Договор, по которому выставили счет
	@AgrManagerId	uniqueidentifier	= NULL,										-- Ответственный по Договору
	@StatusId		uniqueidentifier	= NULL,										-- Статус согласования
	@EPStatusId		uniqueidentifier	= NULL										-- Статус подписания ЭП



--,@PeriodType integer = 5 --Заменить на @DocPeriodType
--,@DateBeg date = NULL --Заменить на @DocDateBeg
--,@DateEnd date = NULL --Заменить на @DocDateEnd
--,@ReportPeriod uniqueidentifier = NULL -- Удалить
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Now datetime = CAST(GetDate() AS date)

	IF @DocDateBeg IS NULL SET @DocDateBeg = '19900101'
	IF @DocDateEnd IS NULL SET @DocDateEnd = '20500101'

	IF      @DocPeriodType = 0 BEGIN SET @DocDateBeg = '19900101'										SET @DocDateEnd = '20500101'                                  END
	ELSE IF @DocPeriodType = 2 BEGIN SET @DocDateBeg = @Now												SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @DocPeriodType = 3 BEGIN SET @DocDateBeg = DateAdd(day, -1, @Now)							SET @DocDateEnd = DateAdd(second, -1, @Now)                   END
	ELSE IF @DocPeriodType = 4 BEGIN SET @DocDateBeg = DateAdd(day, -DateDiff(D, 0, @Now) % 7, @Now)	SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @DocPeriodType = 5 BEGIN SET @DocDateBeg = DateAdd(day, -6, @Now)							SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @DocPeriodType = 6 BEGIN SET @DocDateBeg = DateAdd(day, -DAY(@Now)+1, @Now)					SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @DocPeriodType = 7 BEGIN SET @DocDateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)			SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @DocPeriodType = 8 BEGIN SET @DocDateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)		SET @DocDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	IF @RepDateBeg IS NULL SET @RepDateBeg = '19900101'
	IF @RepDateEnd IS NULL SET @RepDateEnd = '20500101'

	IF      @RepPeriodType = 0 BEGIN SET @RepDateBeg = '19900101'										SET @RepDateEnd = '20500101'                                  END
	ELSE IF @RepPeriodType = 2 BEGIN SET @RepDateBeg = DateAdd(day, -DAY(@Now)+1, @Now)					SET @RepDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @RepPeriodType = 3 BEGIN SET @RepDateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)			SET @RepDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @RepPeriodType = 4 BEGIN SET @RepDateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)		SET @RepDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	SELECT	Id						= D.Id,																									-- Id акта
			DocNumber				= D.DocNumber,																							-- Номер акта
			DocDate					= D.DocDate,																							-- Дата акта
			DocName					= D.CustomerName,																						-- Наименование акта
			DateBeg					= A.DateBeg,																							-- Начало отчетного периода
			DateEnd					= A.DateEnd,																							-- Окончание отчетного периода
			SummaWithNDS			= Q7.SummaWithNDS,																						-- Сумма акта с учетом НДС	
			SummaNDS				= Q7.SummaNDS,																							-- Сумма НДС акта		
			CurrencyName			= C.ValueString,																						-- Валюта счета		
			AgreementId				= D2D_A.Id,																								-- Id Договора
			AgreementName			= CASE WHEN @CompanyId = D2D_A.CustomerCompanyId THEN D2D_A.CustomerName ELSE D2D_A.PerformerName END,	-- Наименование Договора
			CustomerCompanyName		= C_C.ShortName,																						-- Наименование Клиента
			PerformerCompanyName	= C_P.ShortName,																						-- Наименование Экспедитора
			AgrManagerName			= AO.AgrOwnerName,																						-- Ответственный по Договору

			HasAttachmentImgPath	= '/Content/img/status/doc_attach.png',		--Q2.ImageUrl,							-- Путь к изображению статуса "Наличие вложений"
			HasAttachmentHint		= 'Присутствует вложение',															-- Наличие вложений

--			HasAttachment			= Q6.FName,																								-- Наличие вложений
--			ApprovalPerformerImgId	= Q2.StatusId,																							-- Статус согласования Экспедитором
--			ApprovalCustomerImgId	= Q3.StatusId,																							-- Статус согласования Клиентом
--			SigningPerformerImgId	= Q4.StatusId,																							-- Статус подписания ЭП Экспедитором
--			SigningCustomerImgId	= Q5.StatusId,																							-- Статус подписания ЭП Клиентом
			ApprovalPerformerImgId	= Day(D.DocDate) % 3,
			ApprovalCustomerImgId	= Day(D.DocDate) % 4,
			SigningPerformerImgId	= (Day(D.DocDate) % 3) % 2,
			SigningCustomerImgId	= (Day(D.DocDate) % 4) % 2,
			TemplateName			= T.Name																								-- Наименование шаблона акта

	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Act A (nolock) ON A.DocumentId = D.Id
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			LEFT JOIN dbo.vObject2ConditionValue C ON C.TableName = 'dbo.Document' AND C.ObjectId = D.Id AND C.MnemoCode = 'ActCurrency'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_A ON D2D_A.ChildId = D.Id AND D2D_A.ParentType = 'Agreement'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_C ON D2D_C.ChildId = D.Id AND D2D_C.ParentType = 'Claim'
			INNER JOIN dbo.Company C_C (nolock) ON C_C.Id = D.CustomerCompanyId
			INNER JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN dbo.vAgreementOwner AO ON AO.AgreementId = D2D_A.Id /*AND AO.CompanyId = @CompanyId*/ AND AO.RN = 1
			OUTER APPLY
			(
				SELECT	SUM(SummaNDS) AS SummaNDS, SUM(SummaWithNDS) AS SummaWithNDS
				FROM	dbo.ServiceAct SA1 (nolock)
				WHERE	SA1.IsDeleted = 0
					AND	SA1.DocumentId = D.Id
				GROUP BY SA1.DocumentId
			) Q7
			OUTER APPLY
			(
				SELECT top 1 RTrim(RTrim(LTrim(C1.F) + ' ' + Coalesce(C1.I, '')) + ' ' + Coalesce(C1.O, '')) AS FIO, C1.Id AS AgrOwnerId
				FROM	dbo.AgreementOwner AO1 (nolock)
						INNER JOIN dbo.Contact C1 (nolock) ON C1.id = AO1.ContactId
				WHERE	AO1.IsDeleted	= 0
					AND	AO1.AgreementId = D.Id
				ORDER BY Coalesce(DateEnd, '21120110') DESC, Coalesce(DateBeg, '19910110')
			) Q1
			OUTER APPLY
			(
				SELECT top 1 R2.Id AS StatusId, R2.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH2 (nolock)
						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = DSH2.StatusId AND R2.CodeS = 'ACT-TEO-CLI' AND R2.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R2.Id = RI.ReferenceId
				WHERE	DSH2.IsDeleted	= 0
					AND	DSH2.DocumentId	= D.Id
				ORDER BY DSH2.CreatedOn DESC
			) Q2
			OUTER APPLY
			(
				SELECT top 1 R3.Id AS StatusId, R3.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH3 (nolock)
						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = DSH3.StatusId AND R3.CodeS = 'ACT-TEO-CLI' AND R3.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R3.Id = RI.ReferenceId
				WHERE	DSH3.IsDeleted	= 0
					AND	DSH3.DocumentId	= D.Id
				ORDER BY DSH3.CreatedOn DESC
			) Q3
			OUTER APPLY
			(
				SELECT top 1 R4.Id AS StatusId, R4.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH4 (nolock)
						INNER JOIN NSI.Reference R4 (nolock) ON R4.Id = DSH4.StatusId AND R4.CodeS = 'ACT-TEO-EDS' AND R4.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R4.Id = RI.ReferenceId
				WHERE	DSH4.IsDeleted	= 0
					AND	DSH4.DocumentId	= D.Id
				ORDER BY DSH4.CreatedOn DESC
			) Q4
			OUTER APPLY
			(
				SELECT top 1 R5.Id AS StatusId, R5.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH5 (nolock)
						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = DSH5.StatusId AND R5.CodeS = 'ACT-TEO-EDS' AND R5.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R5.Id = RI.ReferenceId
				WHERE	DSH5.IsDeleted	= 0
					AND	DSH5.DocumentId	= D.Id
				ORDER BY DSH5.CreatedOn DESC
			) Q5
			--OUTER APPLY
			--(
			--	SELECT	top 1 CASE WHEN DA6.Id IS NULL THEN NULL ELSE 'B42282D6-FC0A-4071-83A1-9BB413099885' END AS ImageUrl, DA6.FName
			--	FROM	dbo.DocumentAttach DA6 (nolock)
			--	WHERE	DA6.IsDeleted	= 0
			--		AND	DA6.DocumentId	= D.Id
			--) Q6
	WHERE	D.IsDeleted = 0
		AND	( D2D_A.CustomerCompanyId = @CompanyId OR D2D_A.PerformerCompanyId = @CompanyId )
----		AND	( D.DocDate between @DocDateBeg AND @DocDateEnd )
----		AND	( Coalesce(A.DateBeg,'19900101') < @RepDateEnd AND Coalesce(A.DateEnd,'20500101') > @RepDateBeg )
		AND	( @AgreementId IS NULL OR @AgreementId = D2D_A.Id )
		AND	( @AgrManagerId IS NULL OR @AgrManagerId = AO.AgrOwnerId )
	ORDER BY D.DocDate DESC
END
GO
