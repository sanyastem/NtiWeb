USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClaimListGet]
	@CompanyId		uniqueidentifier,				-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256)		= NULL,		-- Группа протоколов (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'CLAIM%')
	@DocPeriodType	integer				= 5,		-- Тип периода для даты документа (0-Все, 1-За период С ПО, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@DocDateBeg		datetime			= NULL,		-- Период даты документа С
	@DocDateEnd		datetime			= NULL,		-- Период даты документа ПО
	@LoadPeriodType	integer				= 7,		-- Тип периода для даты погрузки (0-Все, 1-За период, 2-За сегодня, 3-За вчера, 4-За текущую неделю, 5-За последние 7 дней, 6-За текущий месяц, 7-За текущий квартал, 8-За текущий год)
	@LoadDateBeg	datetime			= NULL,		-- Период даты погрузки С
	@LoadDateEnd	datetime			= NULL,		-- Период даты погрузки ПО
	@AgreementId	uniqueidentifier	= NULL,		-- Договор
	@DocStatusId	uniqueidentifier	= NULL,		-- Статус согласования
	@EDStatusId		uniqueidentifier	= NULL,		-- Статус подписания ЭЦП (Electronic Digital Signature)
	@ContactId		uniqueidentifier	= NULL,		-- Ответственный по Договору
	@ApprovalStatus uniqueidentifier	= NULL,		-- Статус согласования
	@SigningStatus	uniqueidentifier	= NULL		-- Статус подписания

--,@ProtocolGroup nvarchar(256) -- Заменить на @Group
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

	IF @LoadDateBeg IS NULL SET @LoadDateBeg = '19900101'
	IF @LoadDateEnd IS NULL SET @LoadDateEnd = '20500101'

	IF		@LoadPeriodType = 2 BEGIN SET @LoadDateBeg = @Now											SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @LoadPeriodType = 3 BEGIN SET @LoadDateBeg = DateAdd(day, -1, @Now)							SET @LoadDateEnd = DateAdd(second, -1, @Now)                   END
	ELSE IF @LoadPeriodType = 4 BEGIN SET @LoadDateBeg = DateAdd(day, -DateDiff(D, 0, @Now) % 7, @Now)	SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @LoadPeriodType = 5 BEGIN SET @LoadDateBeg = DateAdd(day, -6, @Now)							SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @LoadPeriodType = 6 BEGIN SET @LoadDateBeg = DateAdd(day, -DAY(@Now)+1, @Now)				SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @LoadPeriodType = 7 BEGIN SET @LoadDateBeg = DateAdd(qq, DateDiff(qq, 0, @Now), 0)			SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END
	ELSE IF @LoadPeriodType = 8 BEGIN SET @LoadDateBeg = DateAdd(year, DateDiff(year, 0, @Now), 0)		SET @LoadDateEnd = DateAdd(second, -1, DateAdd(day,  1, @Now)) END

	SELECT	Id						= D.Id,																									-- Id заявки
			StatusName				= R_S.Name,																								-- Текущий статус Заявки
			DocNumber				= D.DocNumber,																							-- Номер заявки
			RegNumber				= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerRegNumber ELSE D.PerformerRegNumber END,	-- Регистрационный номер заявки
			DocDate					= D.DocDate,																							-- Дата заявки
			DocName					= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerName ELSE D.PerformerName END,				-- Наименование заявки
			DateBeg					= C.DateBeg,																							-- Период начала погрузки
			DateEnd					= C.DateEnd,																							-- Период окончания погрузки
--			AgreementId				= A.Id,																									-- Id договора
			AgreementName			= CASE WHEN @CompanyId = A.CustomerCompanyId THEN A.CustomerName ELSE A.PerformerName END,				-- Наименование договора
			StFromCode6				= SF.Code6,																								-- Код станции отправления
			StFromName				= SF.Code6 + '//' + SF.Name + ' (' + SF.RailWayShortName +')',											-- Наименование станции отправления
			RWFromName				= SF.RailWayShortName,																					-- Наименование станции отправления
			StToCode6				= ST.Code6,																								-- Код станции назначения
			StToName				= ST.Code6 + '//' + ST.Name + ' (' + ST.RailWayShortName +')',											-- Наименование станции назначения
			RWToName				= ST.RailWayShortName,																					-- Наименование станции назначения
			CarCount				= C.CarCount,																							-- Вагонов
			FrWeight				= C.FrWeight,																							-- Вес груза (тонн)
			FrETSNGCode				= FrE.Code6,																							-- Груз ЕТСНГ
			FrETSNGName				= FrE.Code6 + '//' + FrE.Name,																			-- Груз ГНГ
			ContactFIO				= Q1.FIO,																								-- ФИО ответственного по договору
			TelegrammList			= '',																									-- Перечень выпущенных телеграмм

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


--,HasAttachment = Q6.FName --Удалить
--,ApprovalPerformerImgId = Q2.StatusId --Удалить
--,ApprovalCustomerImgId = Q3.StatusId --Удалить
--,SigningPerformerImgId = Q4.StatusId --Удалить
--,SigningCustomerImgId = Q5.StatusId --Удалить
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Claim C (nolock) ON C.ClaimId = D.Id
			INNER JOIN NSI.Reference R_S (nolock) ON R_S.Id = D.StatusId
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			INNER JOIN dbo.Document2Document D2D (nolock) ON D2D.ChildId = D.Id AND D2D.ParentType = 'Agreement'
			INNER JOIN dbo.Document A (nolock) ON A.Id = D2D.ParentId
			LEFT JOIN NSI.vStationExt SF (nolock) ON SF.Id = C.StationFromId
			LEFT JOIN NSI.vStationExt ST (nolock) ON ST.Id = C.StationToId
			LEFT JOIN NSI.FrETSNG FrE (nolock) ON FrE.Id = C.FrETSNGId
			OUTER APPLY
			(
				SELECT top 1 RTrim(RTrim(LTrim(C1.F) + ' ' + Coalesce(C1.I, '')) + ' ' + Coalesce(C1.O, '')) AS FIO, C1.Id AS AgrOwnerId
				FROM	dbo.AgreementOwner AO1 (nolock)
						INNER JOIN dbo.Contact C1 (nolock) ON C1.id = AO1.ContactId
				WHERE	AO1.IsDeleted	= 0
					AND	AO1.AgreementId = A.Id
				ORDER BY Coalesce(DateEnd, '21120110') DESC, Coalesce(DateBeg, '19910110')
			) Q1
			OUTER APPLY
			(
				SELECT top 1 R2.Id AS StatusId, R2.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH2 (nolock)
						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = DSH2.StatusId AND R2.CodeS = 'CLAIM-TEO-CLI' AND R2.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R2.Id = RI.ReferenceId
				WHERE	DSH2.IsDeleted	= 0
					AND	DSH2.DocumentId	= D.Id
				ORDER BY DSH2.CreatedOn DESC
			) Q2
			OUTER APPLY
			(
				SELECT top 1 R3.Id AS StatusId, R3.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH3 (nolock)
						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = DSH3.StatusId AND R3.CodeS = 'CLAIM-TEO-CLI' AND R3.ShortName like '%C%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R3.Id = RI.ReferenceId
				WHERE	DSH3.IsDeleted	= 0
					AND	DSH3.DocumentId	= D.Id
				ORDER BY DSH3.CreatedOn DESC
			) Q3
			OUTER APPLY
			(
				SELECT top 1 R4.Id AS StatusId, R4.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH4 (nolock)
						INNER JOIN NSI.Reference R4 (nolock) ON R4.Id = DSH4.StatusId AND R4.CodeS = 'CLAIM-TEO-EDS' AND R4.ShortName like '%E%'
						LEFT JOIN dbo.ReferenceImage RI (nolock) ON R4.Id = RI.ReferenceId
				WHERE	DSH4.IsDeleted	= 0
					AND	DSH4.DocumentId	= D.Id
				ORDER BY DSH4.CreatedOn DESC
			) Q4
			OUTER APPLY
			(
				SELECT top 1 R5.Id AS StatusId, R5.Name, (RI.ImagePath + Coalesce(RI.AddPath, '') + RI.ImageName) AS ImageUrl
				FROM	dbo.DocumentStatusHistory DSH5 (nolock)
						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = DSH5.StatusId AND R5.CodeS = 'CLAIM-TEO-EDS' AND R5.ShortName like '%C%'
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
		AND	( D.CustomerCompanyId = @CompanyId OR D.PerformerCompanyId = @CompanyId )
		AND	( D.DocDate between @DocDateBeg AND @DocDateEnd )
		AND ( @AgreementId IS NULL OR A.Id = @AgreementId )
		AND ( @ApprovalStatus IS NULL OR Q2.StatusId = @ApprovalStatus OR Q3.StatusId = @ApprovalStatus)
		AND ( @SigningStatus IS NULL OR Q4.StatusId = @SigningStatus OR Q5.StatusId = @SigningStatus)
	ORDER BY D.DocDate DESC, D.DocNumber
--SLP_SYNC.dbo.SyncTables
--EXEC [dbo].[ClaimListGet] @CompanyId = '0AE1AA3E-022E-4510-9D9E-C55EADC89547', @ProtocolGroup = 'CLAIM-TEO', @DocPeriodType = 7, @LoadPeriodType = 7--, @AgreementId = 'C0ADA1FD-F572-41D0-A611-A6591BEF0946'
END

GO
