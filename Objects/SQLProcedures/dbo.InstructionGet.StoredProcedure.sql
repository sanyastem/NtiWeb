USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[InstructionGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InstructionGet]
	@CompanyId		uniqueidentifier = NULL,										-- Организация, по отношению к которой выбирается данные Договора
	@Id				uniqueidentifier = NULL										-- Идентификатор Инструкции


--@InstructionId	uniqueidentifier -- Заменить на @Id, после чего параметр удалить
AS
BEGIN
	--SET @Id = @InstructionId
	--SET @CompanyId = '1D1C50D3-99D7-4DB1-9AB5-AE63B6601535'

	SET NOCOUNT ON;

	SELECT	Id						= D.Id,
			TemplateName			= T.Name,																								-- Наименование Шаблона
			DocNumber				= D.DocNumber,																							-- Номер инструкции
			DocDate					= CAST(D.DocDate AS nvarchar),																			-- Дата инструкции
			DocName					= D.CustomerName,																						-- Наименование инструкции
			AgreementId				= D2D_A.Id,																								-- Id Договора
			AgreementName			= CASE WHEN @CompanyId = D2D_A.CustomerCompanyId THEN D2D_A.CustomerName ELSE D2D_A.PerformerName END,	-- Наименование Договора
			CustomerCompanyId		= C_C.Id,																								-- Id компании Клиента
			CustomerCompanyLabel	= Coalesce(O2CV_C.ValueString, 'Заказчик'),																-- Заголовок компании Заказчика
			CustomerCompanyName		= C_C.ShortName,																						-- Наименование компании клиента
			PerformerCompanyId		= C_P.Id,																								-- Id компании Экспедитора
			PerformerCompanyLabel	= Coalesce(O2CV_P.ValueString, 'Исполнитель'),															-- Заголовок компании Исполнителя
			PerformerCompanyName 	= C_P.ShortName,																						-- Наименование компании Экспедитора
			ClaimId					= D2D_C.Id,																								-- Id Заявки
			ClaimName				= D2D_C.CustomerName,																					-- Наименование Заявки
			StationCode				= Coalesce(S.Code6,S.Code5),																			-- Код станции
			RailWayName				= S.RailWayShortName,																					-- Название железной дороги
			StationName				= S.Name,																								-- Название станции

			DocStatusId				= '',																	--Id статуса документа
			DocStatusName			= '',																	--Статус документа																	

			--CreatedBy				= UL_C.Id,																								-- Кем создан Договора (Id)
			CreatedByName			= UL_C.ContactName,																						-- Кем создан Договора (Имя)
			--CreatedOn				= D.CreatedOn,																							-- Дата создания Договора
			--UpdatedBy				= UL_U.Id,																								-- Кем Договор изменялся в последний раз (Id)
			UpdatedByName			= UL_U.ContactName,																						-- Кем Договор изменялся в последний раз (Имя)
			--UpdatedOn				= D.UpdatedOn,																							-- Дата последнего изменения Договора

			NULL AS CreatedBy,
			NULL AS CreatedOn,
			NULL AS UpdatedBy,
			NULL AS UpdatedOn,
			NULL AS IsDeleted,
			IRSPerevozkiId			= irs.ValueInteger																								-- Id Договора в системе ИРС Перевозки
	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Template T (nolock) ON T.Id = D.TemplateId
			LEFT JOIN dbo.Company C_C (nolock) ON C_C.Id = D.CustomerCompanyId
			LEFT JOIN dbo.vObject2ConditionValue O2CV_C ON O2CV_C.TableName = 'dbo.Document' AND O2CV_C.ObjectId = D.Id AND O2CV_C.MnemoCode = 'CustomerLabelName'
			LEFT JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN dbo.vObject2ConditionValue O2CV_P ON O2CV_P.TableName = 'dbo.Document' AND O2CV_P.ObjectId = D.Id AND O2CV_P.MnemoCode = 'PerformerLabelName'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_A ON D2D_A.ChildId = D.Id AND D2D_A.ParentType = 'Agreement'
			LEFT JOIN dbo.vDocument2DocumentGetParent D2D_C ON D2D_C.ChildId = D.Id AND D2D_C.ParentType = 'Claim'
			LEFT JOIN dbo.vObject2ConditionValue D2D_S ON D2D_S.TableName = 'dbo.Document' AND D2D_S.ObjectId = D.Id AND D2D_S.MnemoCode = 'InstructionOnStation'
			LEFT JOIN NSI.vStationExt S ON S.Id = D2D_S.ValueGUID
			INNER JOIN dbo.vUserList UL_C on UL_C.Id = D.CreatedBy
			LEFT JOIN dbo.vUserList UL_U on UL_U.Id = D.UpdatedBy
			LEFT JOIN dbo.vObject2ConditionValue irs ON irs.TableName = 'dbo.Document' AND irs.ObjectId = D.Id AND irs.MnemoCode = 'IRSPerevozkiId_RAT'
	WHERE	D.Id = @Id
END

GO
