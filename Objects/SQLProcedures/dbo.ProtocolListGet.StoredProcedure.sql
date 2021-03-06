USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ProtocolListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ProtocolListGet]
	@CompanyId		uniqueidentifier,				-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256),					-- Группа (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'PROTOCOL%')
	@DocPeriodType	integer				= 5,		-- Тип периода для даты документа (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@DocDateBeg		datetime			= NULL,		-- Период даты документа С
	@DocDateEnd		datetime			= NULL,		-- Период даты документа ПО										
	@UsePeriodType	integer				= 5,		-- Тип периода для срока действия (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@UseDateBeg		datetime			= NULL,		-- Период действия С
	@UseDateEnd		datetime			= NULL,		-- Период действия ПО										
	@AgreementId	uniqueidentifier	= NULL,		-- Договор, по которому выставили счет
	@AgrManagerId	uniqueidentifier	= NULL,		-- Ответственный по Договору
	@StatusId		uniqueidentifier	= NULL,		-- Статус согласования
	@EPStatusId		uniqueidentifier	= NULL		-- Статус подписания ЭП
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

	IF @UseDateBeg IS NULL SET @UseDateBeg = '19900101'
	IF @UseDateEnd IS NULL SET @UseDateEnd = '20500101'

	IF		@UsePeriodType = 2 BEGIN SET @UseDateBeg = @Now												SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @UsePeriodType = 3 BEGIN SET @UseDateBeg = DateAdd(day, -1, @Now)							SET @UseDateEnd = DateAdd(second, -1, @Now)                   END
	ELSE IF @UsePeriodType = 4 BEGIN SET @UseDateBeg = DateAdd(day, -DateDiff(D, 0, @Now) % 7, @Now)	SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @UsePeriodType = 5 BEGIN SET @UseDateBeg = DateAdd(day, -6, @Now)							SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @UsePeriodType = 6 BEGIN SET @UseDateBeg = DateAdd(day, -DAY(@Now)+1, @Now)					SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @UsePeriodType = 7 BEGIN SET @UseDateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)			SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @UsePeriodType = 8 BEGIN SET @UseDateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)		SET @UseDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	SELECT	Id						= D.Id,																									-- Id протокола
			DocNumber				= D.DocNumber,																							-- Номер протокола
			--RegNumber				= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerRegNumber ELSE D.PerformerRegNumber END,	-- Рег. номер протокола
			DocDate					= D.DocDate,																							-- Дата протокола
			DocName					= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerName ELSE D.PerformerName END,				-- Наименование протокола
			DateBeg					= A.DateBeg,																							-- Период действия С
			DateEnd					= Coalesce(A.DateStop, A.DateEnd),																		-- Период действия ПО
			AgreementName			= CASE WHEN @CompanyId = D2D_A.CustomerCompanyId THEN D2D_A.CustomerName ELSE D2D_A.PerformerName END,	-- Наименование договора
			ClaimName				= D2D_C.CustomerName,																					-- Наименование заявки
			CustomerCompanyName		= C_C.ShortName,																						-- Наименование Клиента
			PerformerCompanyName	= C_P.ShortName,																						-- Наименование Экспедитора
			AgrManagerName			= AO.AgrOwnerName,																						-- Менеджер по договору
			TemplateName			= T.Name,																								-- Наименование шаблона протокола

			HasAttachmentImgPath	= '/Content/img/status/doc_attach.png',		--Q2.ImageUrl,								-- Путь к изображению статуса "Наличие вложений"
			HasAttachmentHint		= 'Присутствует вложение',															-- Наличие вложений

			ApprovalPerformerImgPath	= Q2.ImageUrl,
			ApprovalCustomerImgPath		= Q3.ImageUrl,
			SigningPerformerImgPath		= Q4.ImageUrl,
			SigningCustomerImgPath		= Q5.ImageUrl,

			ApprovalPerformerHint		= Q2.Name,																								-- Статус согласования Экспедитором
			ApprovalCustomerHint		= Q3.Name,																								-- Статус согласования Клиентом
			SigningPerformerHint		= Q4.Name,																								-- Статус подписания ЭП Экспедитором
			SigningCustomerHint			= Q5.Name																								-- Статус подписания ЭП Клиентом


,HasAttachment = Q6.FName --Удалить
,ApprovalPerformerImgId	= Day(D.DocDate) % 3 --Удалить
,ApprovalCustomerImgId	= Day(D.DocDate) % 4 --Удалить
,SigningPerformerImgId	= (Day(D.DocDate) % 3) % 2 --Удалить
,SigningCustomerImgId	= (Day(D.DocDate) % 4) % 2 --Удалить
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Agreement A (nolock) ON A.DocumentId = D.Id
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			INNER JOIN dbo.Company C_C (nolock) ON C_C.Id = D.CustomerCompanyId
			INNER JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN NSI.Reference R ON R.Id = D.StatusId
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_A ON D2D_A.ChildId = D.Id AND D2D_A.ParentType = 'Agreement'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_C ON D2D_C.ChildId = D.Id AND D2D_C.ParentType = 'Claim'
			LEFT JOIN dbo.vAgreementOwner AO ON AO.AgreementId = D2D_A.Id /*AND AO.CompanyId = @CompanyId*/ AND AO.RN = 1
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
			) Q2
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
			) Q3
			OUTER APPLY
			(
				SELECT top 1 R4.Id AS StatusId, R4.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH4 (nolock)
						INNER JOIN NSI.Reference R4 (nolock) ON R4.Id = 'B42282D6-FC0A-4071-83A1-9BB413099885'
--						INNER JOIN NSI.Reference R4 (nolock) ON R4.Id = DSH4.StatusId AND R4.CodeS = 'ACT-TEO-EDS' AND R4.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R4.Id = RI.ReferenceId
				WHERE	DSH4.IsDeleted	= 0
					AND	DSH4.DocumentId	= D.Id
				ORDER BY DSH4.CreatedOn DESC
			) Q4
			OUTER APPLY
			(
				SELECT top 1 R5.Id AS StatusId, R5.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH5 (nolock)
						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = 'B42282D6-FC0A-4071-83A1-9BB413099885'
--						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = DSH5.StatusId AND R5.CodeS = 'ACT-TEO-EDS' AND R5.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R5.Id = RI.ReferenceId
				WHERE	DSH5.IsDeleted	= 0
					AND	DSH5.DocumentId	= D.Id
				ORDER BY DSH5.CreatedOn DESC
			) Q5
			OUTER APPLY
			(
				SELECT	top 1 CASE WHEN DA6.Id IS NULL THEN NULL ELSE 'B42282D6-FC0A-4071-83A1-9BB413099885' END AS ImageUrl, DA6.FName
				FROM	dbo.DocumentAttach DA6 (nolock)
				WHERE	DA6.IsDeleted	= 0
					AND	DA6.DocumentId	= D.Id
			) Q6
	WHERE	D.IsDeleted = 0
		AND	( D2D_A.CustomerCompanyId = @CompanyId OR D2D_A.PerformerCompanyId = @CompanyId )
		AND	( D.DocDate between @DocDateBeg AND @DocDateEnd )
	ORDER BY DocName
END

GO
