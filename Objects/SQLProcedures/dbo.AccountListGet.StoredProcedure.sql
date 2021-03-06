USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AccountListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AccountListGet]
	@CompanyId		uniqueidentifier,					-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256),						-- Группа (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'ACCOUNT%')
	@PeriodType		integer				= 5,			-- Тип периода (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@DateBeg		datetime			= NULL,			-- Период С
	@DateEnd		datetime			= NULL,			-- Период ПО										
	@ReportPeriod	uniqueidentifier	= NULL,			-- Отчетный период
	@AgreementId	uniqueidentifier	= NULL,			-- Договор, по которому выставили счет
	@AgrManagerId	uniqueidentifier	= NULL,			-- Ответственный по Договору
	@StatusId		uniqueidentifier	= NULL,			-- Статус согласования
	@EPStatusId		uniqueidentifier	= NULL			-- Статус подписания ЭП
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Now datetime = CAST(GetDate() AS date)

	IF @DateBeg IS NULL SET @DateBeg = '19900101'
	IF @DateEnd IS NULL SET @DateBeg = '20500101'

	IF      @PeriodType = 0 BEGIN SET @DateBeg = '19900101'										SET @DateEnd = '20500101'                                  END
	ELSE IF @PeriodType = 2 BEGIN SET @DateBeg = @Now											SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 3 BEGIN SET @DateBeg = DateAdd(day, -1, @Now)							SET @DateEnd = DateAdd(second, -1, @Now)                   END
	ELSE IF @PeriodType = 4 BEGIN SET @DateBeg = DateAdd(day, -DateDiff(D, 0, @Now) % 7, @Now)	SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 5 BEGIN SET @DateBeg = DateAdd(day, -6, @Now)							SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 6 BEGIN SET @DateBeg = DateAdd(day, -DAY(@Now)+1, @Now)				SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 7 BEGIN SET @DateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)			SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 8 BEGIN SET @DateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)		SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	SELECT	Id						= D.Id,																												-- Id счета
			DocNumber				= D.DocNumber,																										-- Номер счета
			DocDate					= D.DocDate,																										-- Дата счета
			DocName					= D.CustomerName,																									-- Наименование счета
			ReportPeriod			= RP.ValueString,																									-- Отчетный период
			SummaWithNDS			= Q1.SummaWithNDS,																									-- Сумма счета с учетом НДС
			SummaNDS				= Q1.SummaNDS,																										-- Сумма НДС счета
			CurrencyName			= C.ValueString,																									-- Валюта счета
			AgreementName			= CASE WHEN @CompanyId = D2D_A.CustomerCompanyId THEN D2D_A.CustomerName ELSE D2D_A.PerformerName END,				-- Наименование договора
			ClaimName				= D2D_C.CustomerName,																								-- Наименование заявки
			CustomerCompanyName		= C_C.ShortName,																									-- Наименование Клиента
			PerformerCompanyName	= C_P.ShortName,																									-- Наименование Экспедитора
			AgrManagerName			= AO.AgrOwnerName,																									-- Менеджер по договору
			Signer1Name				= '_____ _. _.',																									-- Подписант 1 счета
			Signer2Name				= '_____ _. _.',																									-- Подписант 2 счета
			TemplateName			= T.Name,																											-- Наименование шаблона счета

			HasAttachmentImgPath	= '/Content/img/status/doc_attach.png',		--Q2.ImageUrl,							-- Путь к изображению статуса "Наличие вложений"
			HasAttachmentHint		= 'Присутствует вложение',															-- Наличие вложений

			ApprovalAccountImgPath	= '/Content/img/status/01_doc.png',		--Q2.ImageUrl,							-- Путь к изображению статуса согласования Экспедитором
			SigningAccountImgPath	= '/Content/img/status/04_doc.png',		--Q3.ImageUrl,							-- Путь к изображению статуса согласования Клиентом
			ApprovalClaimImgPath	= '/Content/img/status/sign_perf.png',	--Q4.ImageUrl,							-- Путь к изображению статуса подписания ЭП Экспедитором
			ApprovalProtocolImgPath	= '/Content/img/status/sign_cust.png',	--Q5.ImageUrl,							-- Путь к изображению статуса подписания ЭП Клиентом
			SigningClaimImgPath		= '/Content/img/status/sign_perf.png',	--Q4.ImageUrl,							-- Путь к изображению статуса подписания ЭП Экспедитором
			SigningProtocolPath		= '/Content/img/status/sign_cust.png',	--Q5.ImageUrl,							-- Путь к изображению статуса подписания ЭП Клиентом

			ApprovalAccountHint		= 'Согласована ЗАО «Русагротранс»; Согласована Клиентом',						-- Статус согласования Экспедитором
			SigningAccountHint		= 'Согласована ЗАО «Русагротранс»; На согласовании Клиента',					-- Статус согласования Клиентом
			ApprovalClaimHint		= 'Заявка согласована',															-- Статус подписания ЭП Экспедитором
			ApprovalProtocolHint	= 'Протокол согласован',														-- Статус подписания ЭП Клиентом
			SigningClaimHint		= 'Заявка подписана ЭП',														-- Статус подписания ЭП Экспедитором
			SigningProtocolHint		= 'Протокол подписан ЭП'														-- Статус подписания ЭП Клиентом


--,HasAttachment			= Q7.FName --Удалить
--,ApprovalAccountImgId	= Day(D.DocDate) % 3 --Удалить
--,SigningAccountImgId		= Day(D.DocDate) % 3 --Удалить
--,ApprovalClaimImgId		= Day(D.DocDate) % 3 --Удалить
--,ApprovalProtocolImgId	= Day(D.DocDate) % 3 --Удалить
--,SigningClaimImgId		= Day(D.DocDate) % 3 --Удалить
--,SigningProtocolImgId	= Day(D.DocDate) % 3 --Удалить
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			INNER JOIN dbo.Company C_C (nolock) ON C_C.Id = D.CustomerCompanyId
			INNER JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN dbo.vObject2ConditionValue RP ON RP.TableName = 'dbo.Document' AND RP.ObjectId = D.Id AND RP.MnemoCode = 'ReportPeriod'
			LEFT JOIN dbo.vObject2ConditionValue C ON C.TableName = 'dbo.Document' AND C.ObjectId = D.Id AND C.MnemoCode = 'AccountCurrency'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_A ON D2D_A.ChildId = D.Id AND D2D_A.ParentType = 'Agreement'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_C ON D2D_C.ChildId = D.Id AND D2D_C.ParentType = 'Claim'
			LEFT JOIN dbo.vAgreementOwner AO ON AO.AgreementId = D2D_A.Id /*AND AO.CompanyId = @CompanyId*/ AND AO.RN = 1
			OUTER APPLY
			(
				SELECT	SUM(SummaNDS) AS SummaNDS, SUM(SummaWithNDS) AS SummaWithNDS
				FROM	dbo.ServiceAccount SA1 (nolock)
				WHERE	SA1.IsDeleted = 0
					AND	SA1.DocumentId = D.Id
				GROUP BY SA1.DocumentId
			) Q1
			OUTER APPLY
			(
				SELECT top 1 RTrim(RTrim(LTrim(C2.F) + ' ' + Coalesce(C2.I, '')) + ' ' + Coalesce(C2.O, '')) AS FIO, C2.Id AS AgrOwnerId
				FROM	dbo.AgreementOwner AO2 (nolock)
						INNER JOIN dbo.Contact C2 (nolock) ON C2.id = AO2.ContactId
				WHERE	AO2.IsDeleted	= 0
					AND	AO2.AgreementId = D.Id
				ORDER BY Coalesce(AO2.DateEnd, '21120110') DESC, Coalesce(AO2.DateBeg, '19910110')
			) Q2
			OUTER APPLY
			(
				SELECT top 1 R2.Id AS StatusId, R2.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH2 (nolock)
						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = 'D31DB2CD-E67D-4AB9-A67A-0BA97C571941'
--						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = DSH2.StatusId AND R2.CodeS = 'ACT-TEO-CLI' AND R2.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R2.Id = RI.ReferenceId
				WHERE	DSH2.IsDeleted	= 0
					AND	DSH2.DocumentId	= D.Id
				ORDER BY DSH2.CreatedOn DESC
			) Q3
			OUTER APPLY
			(
				SELECT top 1 R3.Id AS StatusId, R3.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH3 (nolock)
						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = 'B42282D6-FC0A-4071-83A1-9BB413099885'
--						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = DSH3.StatusId AND R3.CodeS = 'ACT-TEO-CLI' AND R3.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R3.Id = RI.ReferenceId
				WHERE	DSH3.IsDeleted	= 0
					AND	DSH3.DocumentId	= D.Id
				ORDER BY DSH3.CreatedOn DESC
			) Q4
			OUTER APPLY
			(
				SELECT top 1 R5.Id AS StatusId, R5.Name, (RI5.ImagePath + Coalesce(RI5.AddPath, '') + RI5.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH5 (nolock)
						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = 'B42282D6-FC0A-4071-83A1-9BB413099885'
--						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = DSH5.StatusId AND R5.CodeS = 'ACT-TEO-EDS' AND R5.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI5 (nolock) ON R5.Id = RI5.ReferenceId
				WHERE	DSH5.IsDeleted	= 0
					AND	DSH5.DocumentId	= D.Id
				ORDER BY DSH5.CreatedOn DESC
			) Q5
			OUTER APPLY
			(
				SELECT top 1 R6.Id AS StatusId, R6.Name, (RI6.ImagePath + Coalesce(RI6.AddPath, '') + RI6.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH6 (nolock)
						INNER JOIN NSI.Reference R6 (nolock) ON R6.Id = 'B42282D6-FC0A-4071-83A1-9BB413099885'
--						INNER JOIN NSI.Reference R6 (nolock) ON R6.Id = DSH6.StatusId AND R6.CodeS = 'ACT-TEO-EDS' AND R6.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI6 (nolock) ON R6.Id = RI6.ReferenceId
				WHERE	DSH6.IsDeleted	= 0
					AND	DSH6.DocumentId	= D.Id
				ORDER BY DSH6.CreatedOn DESC
			) Q6
			--OUTER APPLY
			--(
			--	SELECT	top 1 CASE WHEN DA6.Id IS NULL THEN NULL ELSE 'B42282D6-FC0A-4071-83A1-9BB413099885' END AS ImageUrl, DA6.FName
			--	FROM	dbo.DocumentAttach DA6 (nolock)
			--	WHERE	DA6.IsDeleted	= 0
			--		AND	DA6.DocumentId	= D.Id
			--) Q7
	WHERE	D.IsDeleted = 0
		AND	( D.CustomerCompanyId = @CompanyId OR D.PerformerCompanyId = @CompanyId )
		AND	( D.DocDate between @DateBeg AND @DateEnd )
		AND	( @ReportPeriod IS NULL OR @ReportPeriod = RP.ValueGUID )
		AND	( @AgreementId IS NULL OR @AgreementId = D2D_A.Id )
		AND	( @AgrManagerId IS NULL OR @AgrManagerId = AO.AgrOwnerId )
END
GO
