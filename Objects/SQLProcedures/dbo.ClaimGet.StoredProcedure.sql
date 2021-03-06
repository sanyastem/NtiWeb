USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClaimGet] (
		@ClaimId		uniqueidentifier,														--Id заявки '21DB81C7-C949-4E34-9CE5-004AD6EB492E'
		@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
		@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
)
AS
BEGIN
	SET LANGUAGE russian; -- Нужно для отображения дат полного периода на русском языке

	SET NOCOUNT ON;

	SELECT	Id						= D.Id,																									-- Id заявки
			TemplateName			= T.Name,																								-- ? Шаблон
			DocStatusId				= R_S.Id,																								-- Статус заявки Id																					
			DocStatusName			= R_S.Name,																								-- Текущий статус Заявки
			DocNumber				= D.DocNumber,																							-- Номер заявки
			RegNumber				= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerRegNumber ELSE D.PerformerRegNumber END,	-- Регистрационный номер заявки
			DocDate					= D.DocDate,																							-- Дата заявки
			DateBeg					= C.DateBeg,																							-- Период начала погрузки
			DateEnd					= C.DateEnd,																							-- Период окончания погрузки
			ReportPeriodName		= DATENAME(MONTH, C.DateBeg) + '-' + DATENAME(MONTH, C.DateEnd) + ' ' + DATENAME(YEAR, C.DateEnd),		-- Полный период погрузки
			PlanType				= '',																									-- заглушка Тип плана
			AgreementId				= A.Id,																									-- Id договора
			AgreementName			= CASE WHEN @CompanyId = A.CustomerCompanyId THEN A.CustomerName ELSE A.PerformerName END,				-- Наименование договора

			CountryFromName			= SF.CountryShortName,																					-- ? Страна отправления
			CountryToName			= ST.CountryShortName,																					-- ? Страна назначения
			
			StationFromId			= SF.Id,
			StationFromCode			= SF.Code6,																								-- Код станции отправления
			RailWayFromName			= SF.RailWayShortName,																					-- Наименование станции отправления			
			StationFromName			= SF.Name,																								-- Наименование станции отправления
			
			StationToId				= ST.Id,
			StationToCode			= ST.Code6,																								-- Код станции назначения
			RailWayToName			= ST.RailWayShortName,																					-- Наименование станции назначения
			StationToName			= ST.Name,																								-- Наименование станции назначения			
			
			TransferTypeName		= '',																									-- заглушка Вид сообщения
			Distance				= 120.5,																								-- заглушка Тарифное расстояние (км)
			PeriodOfDelivery		= 0,																									-- заглушка Нормативный срок доставки (дней), int
			
			RouteName				= SF.Name + '->' + ST.Name,																				-- <Детализация маршрута>																					

			FrETSNGCode				= FrE.Code6,																							-- Груз ЕТСНГ
			FrETSNGName				= FrE.Name,																								-- Груз ЕТСНГ

			FrGNGCode				= FrG.Code,																								-- ? Груз ГНГ
			FrGNGName				= FrG.Name,																								-- ? Груз ГНГ
																								
			LoadFromTGNL			= LoaderFromTGNL,																						-- ? Грузоотправитель
			LoadFromName			= CF.ShortName,																							-- ? Грузоотправитель	
													
			LoadToTGNL				= LoaderToTGNL,																							-- ? Грузополучатель
			LoadToName				= CT.ShortName,																							-- ? Грузополучатель
														
			FrWeight				= C.FrWeight,																							-- Вес груза (тонн)
			ContCount				= C.ContCount,																							-- ? Количество контейнеров, int
			CarCount				= C.CarCount,																							-- Вагонов
			
			ClaimNote				= '',																									-- заглушка Примечание

			CreatedBy				= C.CreatedBy,
			CreatedByName			= cusr.ContactName,
			CreatedOn				= C.CreatedOn,
			UpdatedBy				= C.UpdatedBy,
			UpdatedByName			= uusr.ContactName,
			UpdatedOn				= C.UpdatedOn,
			
			IRSPerevozkiId			= irs.ValueInteger																						-- Id Заявки в системе ИРС Перевозки

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
			LEFT JOIN NSI.FrGNG FrG (nolock) ON FrG.Id = C.FrGNGId
			LEFT JOIN dbo.Company CF (nolock) ON CF.Id = LoaderFromId
			LEFT JOIN dbo.Company CT (nolock) ON CT.Id = LoaderToId

			INNER JOIN dbo.vUserList cusr on cusr.Id = D.CreatedBy
			LEFT JOIN dbo.vUserList uusr on uusr.Id = D.UpdatedBy

			LEFT JOIN dbo.vObject2ConditionValue irs ON irs.TableName = 'dbo.Document' AND irs.ObjectId = D.Id AND irs.MnemoCode = 'IRSPerevozkiId_RAT'

	WHERE	( D.Id = @ClaimId )
END

GO
