USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimDetailsGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClaimDetailsGet]
	@Id				uniqueidentifier,
	@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
	@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
AS
BEGIN
	SET LANGUAGE russian; -- Нужно для отображения дат полного периода на русском языке
	
	SET NOCOUNT ON;

    SELECT	Id						= D.Id,																									-- Id заявки
			StatusName				= R_S.Name,																								-- Текущий статус Заявки
			DocNumber				= D.DocNumber,																							-- Номер заявки
			RegNumber				= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerRegNumber ELSE D.PerformerRegNumber END,	-- Регистрационный номер заявки
			DocDate					= D.DocDate,																							-- Дата заявки
			DocName					= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerName ELSE D.PerformerName END,				-- Наименование заявки
			DateBeg					= C.DateBeg,																							-- Период начала погрузки
			DateEnd					= C.DateEnd,																							-- Период окончания погрузки
			DatePeriod				= DATENAME(MONTH, C.DateBeg) + '-' + DATENAME(MONTH, C.DateEnd) + ' ' + DATENAME(YEAR, C.DateEnd),		-- Полный период погрузки
			AgreementId				= A.Id,																									-- Id договора
			AgreementName			= CASE WHEN @CompanyId = A.CustomerCompanyId THEN A.CustomerName ELSE A.PerformerName END,				-- Наименование договора
			StFromId				= SF.Id,
			StFromCode6				= SF.Code6,																								-- Код станции отправления
			StFromName				= SF.Name,																								-- Наименование станции отправления
			RWFromName				= SF.RailWayShortName,																					-- Наименование станции отправления
			StToId					= ST.Id,
			StToCode6				= ST.Code6,																								-- Код станции назначения
			StToName				= ST.Name,																								-- Наименование станции назначения
			RWToName				= ST.RailWayShortName,																					-- Наименование станции назначения
			CarCount				= C.CarCount,																							-- Вагонов
			FrWeight				= C.FrWeight,																							-- Вес груза (тонн)
			FrETSNGCode				= FrE.Code6,																							-- Вес груза (тонн)
			FrETSNGName				= FrE.Name,																								-- Вес груза (тонн)
			ContactId				= Q1.AgrOwnerId,																						-- Id ответственного по договору
			ContactFIO				= Q1.FIO,																								-- ФИО ответственного по договору
			TelegrammList			= '',																									-- Перечень выпущенных телеграмм
			ApprovalPerformerImgId	= Q2.StatusId,																							-- Статус согласования Экспедитором			
			ApprovalCustomerImgId	= Q3.StatusId,																							-- Статус согласования Клиентом			
			SigningPerformerImgId	= Q4.StatusId,																							-- Статус подписания ЭП Экспедитором
			SigningCustomerImgId	= Q5.StatusId,																							-- Статус подписания ЭП Клиентом


			ApprovalPerformer		= Day(D.DocDate) % 3,																					-- Статус согласования Экспедитором
			ApprovalPerformerMemo	= Q2.Name,
			ApprovalCustomer		= Day(D.DocDate) % 4,																					-- Статус согласования Клиентом
			ApprovalCustomerMemo	= Q3.Name,
			SigningPerformer		= (Day(D.DocDate) % 3) % 2,																				-- Статус подписания ЭП Экспедитором
			SigningPerformerMemo	= Q4.Name,
			SigningCustomer			= (Day(D.DocDate) % 4) % 2,																				-- Статус подписания ЭП Клиентом
			SigningCustomerMemo		= Q5.Name
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Claim C (nolock) ON C.ClaimId = D.Id
			INNER JOIN NSI.Reference R_S (nolock) ON R_S.Id = D.StatusId
			INNER JOIN dbo.Template T ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@ProtocolGroup IS NULL OR TT.CodeS = @ProtocolGroup)
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
				SELECT top 1 DSH2.StatusId, R2.Name
				FROM	dbo.DocumentStatusHistory DSH2 (nolock)
						INNER JOIN NSI.Reference R2 (nolock) ON R2.Id = DSH2.StatusId AND R2.CodeS = 'CLAIM-TEO-CLI' AND R2.ShortName like '%E%'
				WHERE	DSH2.IsDeleted	= 0
					AND	DSH2.DocumentId	= D.Id
				ORDER BY DSH2.CreatedOn DESC
			) Q2
			OUTER APPLY
			(
				SELECT top 1 DSH3.StatusId, R3.Name
				FROM	dbo.DocumentStatusHistory DSH3 (nolock)
						INNER JOIN NSI.Reference R3 (nolock) ON R3.Id = DSH3.StatusId AND R3.CodeS = 'CLAIM-TEO-CLI' AND R3.ShortName like '%C%'
				WHERE	DSH3.IsDeleted	= 0
					AND	DSH3.DocumentId	= D.Id
				ORDER BY DSH3.CreatedOn DESC
			) Q3
			OUTER APPLY
			(
				SELECT top 1 DSH4.StatusId, R4.Name
				FROM	dbo.DocumentStatusHistory DSH4 (nolock)
						INNER JOIN NSI.Reference R4 (nolock) ON R4.Id = DSH4.StatusId AND R4.CodeS = 'CLAIM-TEO-EDS' AND R4.ShortName like '%E%'
				WHERE	DSH4.IsDeleted	= 0
					AND	DSH4.DocumentId	= D.Id
				ORDER BY DSH4.CreatedOn DESC
			) Q4
			OUTER APPLY
			(
				SELECT top 1 DSH5.StatusId, R5.Name
				FROM	dbo.DocumentStatusHistory DSH5 (nolock)
						INNER JOIN NSI.Reference R5 (nolock) ON R5.Id = DSH5.StatusId AND R5.CodeS = 'CLAIM-TEO-EDS' AND R5.ShortName like '%C%'
				WHERE	DSH5.IsDeleted	= 0
					AND	DSH5.DocumentId	= D.Id
				ORDER BY DSH5.CreatedOn DESC
			) Q5
	WHERE	( D.Id = @Id )
END

GO
