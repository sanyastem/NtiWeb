USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[InstructionListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InstructionListGet]
	@CompanyId		uniqueidentifier,												-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256),													-- Группа (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'INSTRUCTION%')
	@PeriodType		integer				= 5,										-- Тип периода (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@DateBeg		date				= NULL,										-- Период С
	@DateEnd		date				= NULL,										-- Период ПО										
	@StationId		uniqueidentifier	= NULL,										-- Станция, по которой выдана инструкция
	@AgreementId	uniqueidentifier	= NULL,										-- Договор, по которому выдана инструкция
	@AgrManagerId	uniqueidentifier	= NULL,										-- Ответственный по Договору
	@TemplateId		uniqueidentifier	= NULL,										-- Тип (шаблон) инструкции
	@StatusId		uniqueidentifier	= NULL,										-- Статус согласования
	@EPStatusId		uniqueidentifier	= NULL										-- Статус подписания ЭП
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Now datetime = CAST(GetDate() AS date)

	IF @DateBeg IS NULL SET @DateBeg = '19900101'
	IF @DateEnd IS NULL SET @DateEnd = '20500101'

	IF      @PeriodType = 0 BEGIN SET @DateBeg = '19900101'											SET @DateEnd = '20500101'                                  END
	ELSE IF @PeriodType = 2 BEGIN SET @DateBeg = @Now												SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 3 BEGIN SET @DateBeg = DateAdd(day, -1, @Now)								SET @DateEnd = DateAdd(second, -1, @Now)                   END
	ELSE IF @PeriodType = 4 BEGIN SET @DateBeg = DateAdd(day, -DateDiff(D, 0, @Now) % 7, @Now)		SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 5 BEGIN SET @DateBeg = DateAdd(day, -6, @Now)								SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 6 BEGIN SET @DateBeg = DateAdd(day, -DAY(@Now)+1, @Now)					SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 7 BEGIN SET @DateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)				SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @PeriodType = 8 BEGIN SET @DateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)			SET @DateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	SELECT	Id						= D.Id,								-- Id инструкции
			DocNumber				= D.DocNumber,						-- Номер инструкции
			DocDate					= D.DocDate,						-- Дата инструкции
			DocName					= D.CustomerName,					-- Наименование инструкции
			AgreementName			= CASE WHEN @CompanyId = D2D_A.CustomerCompanyId THEN D2D_A.CustomerName ELSE D2D_A.PerformerName END,	-- Наименование договора
			ClaimName				= D2D_C.CustomerName,																					-- Наименование заявки
			CustomerCompanyName		= C.ShortName,						-- Клиент
			PerformerCompanyName	= C_P.ShortName,					-- Наименование Экспедитора
			AgrManagerName			= AO.AgrOwnerName,
			
--			HasAttachment			= Q3.FName,																								-- Наличие вложений

			HasAttachmentImgPath	= '/Content/img/status/doc_attach.png',		--Q2.ImageUrl,							-- Путь к изображению статуса "Наличие вложений"
			HasAttachmentHint		= 'Присутствует вложение',															-- Наличие вложений

--			ApprovalInstructionImgId	= Day(D.DocDate) % 3,	
--			SigningInstructionImgId		=(Day(D.DocDate) % 3) % 2,

			ApprovalInstructionHint		= 'Согласована ЗАО «Русагротранс»; Согласована Клиентом',
			SigningInstructionHint		= 'Подписано ЭП Экспедитора',
			ApprovalInstructionImgPath	= '/Content/img/status/01_doc.png',
			SingingInstructionImgPath	= '/Content/img/status/sign_perf.png',

			TemplateName				= T.Name
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			INNER JOIN dbo.Company C (nolock) ON C.Id = D.CustomerCompanyId
			INNER JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_A ON D2D_A.ChildId = D.Id AND D2D_A.ParentType = 'Agreement'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_C ON D2D_C.ChildId = D.Id AND D2D_C.ParentType = 'Claim'
			LEFT JOIN dbo.vAgreementOwner AO ON AO.AgreementId = D2D_A.Id /*AND AO.CompanyId = @CompanyId*/ AND AO.RN = 1
			OUTER APPLY
			(
				SELECT top 1 R2.Id AS StatusId, R2.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH2 (nolock)
						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = 'D7087D6A-EB7A-4FBA-96DC-2B1A801D6821'
--						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = DSH2.StatusId AND R2.CodeS = 'CLAIM-TEO-CLI' AND R2.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R2.Id = RI.ReferenceId
				WHERE	DSH2.IsDeleted	= 0
					AND	DSH2.DocumentId	= D.Id
				ORDER BY DSH2.CreatedOn DESC
			) Q1
			OUTER APPLY
			(
				SELECT top 1 R3.Id AS StatusId, R3.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH3 (nolock)
						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = '89C63A98-956C-464F-8F3B-EA7F38A2F31E'
--						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = DSH3.StatusId AND R3.CodeS = 'CLAIM-TEO-CLI' AND R3.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R3.Id = RI.ReferenceId
				WHERE	DSH3.IsDeleted	= 0
					AND	DSH3.DocumentId	= D.Id
				ORDER BY DSH3.CreatedOn DESC
			) Q2
			--OUTER APPLY
			--(
			--	SELECT	top 1 CASE WHEN DA6.Id IS NULL THEN NULL ELSE 'B42282D6-FC0A-4071-83A1-9BB413099885' END AS ImageUrl, DA6.FName
			--	FROM	dbo.DocumentAttach DA6 (nolock)
			--	WHERE	DA6.IsDeleted	= 0
			--		AND	DA6.DocumentId	= D.Id
			--) Q3
	WHERE	D.IsDeleted = 0
		AND	D.DocDate between @DateBeg AND @DateEnd
END

GO
