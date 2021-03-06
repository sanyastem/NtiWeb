USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[AgreementListGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgreementListGet]
	@CompanyId		uniqueidentifier,				-- Организация, по отношению к которой выбираются Договоры (Обязательное поле)
	@Group			nvarchar(256),					-- Группа протоколов (SELECT * FROM NSI.Reference WHERE RefTypeId = '73E294FB-2896-E611-80C3-0018FE2F231E' AND CodeS like 'CLAIM%')
	@AgrManagerId	uniqueidentifier	= NULL,		-- Менеджер, ответственный по Договору
	@TemplateId		uniqueidentifier	= NULL,		-- Шаблон Договора
	@OnlyActive		bit					= 1			-- Признак "Только действующие" (0 - Все договоры, 1 - Только действующие)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	Id						= D.Id,																										--
			DocNumber				= D.DocNumber,																								--
			RegNumber				= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerRegNumber ELSE D.PerformerRegNumber END,		--
			DocDate					= D.DocDate,																								--
			Balance					= 0.0,																										-- Оперативное сальдо
			DocName					= CASE WHEN @CompanyId = D.CustomerCompanyId THEN D.CustomerName ELSE D.PerformerName END,					--
			DateBeg					= A.DateBeg,																								--
			DateEnd					= Coalesce(A.DateStop, A.DateEnd),																			--
			CustomerCompanyName		= C_C.ViewName,																								--
			PerformerCompanyName	= C_P.ViewName,																								--
			AgrManagerName			= AO.AgrOwnerName,																									-- Менеджер по договору

			HasAttachmentImgPath	= '/Content/img/status/doc_attach.png',		--Q2.ImageUrl,							-- Путь к изображению статуса "Наличие вложений"
			HasAttachmentHint		= 'Присутствует вложение',															-- Наличие вложений

			ApprovalPerformerImgPath	= '/Content/img/status/01_doc.png',		--Q2.ImageUrl,							-- Путь к изображению статуса согласования Экспедитором
			ApprovalCustomerImgPath		= '/Content/img/status/04_doc.png',		--Q3.ImageUrl,							-- Путь к изображению статуса согласования Клиентом
			SigningPerformerImgPath		= '/Content/img/status/sign_perf.png',	--Q4.ImageUrl,							-- Путь к изображению статуса подписания ЭП Экспедитором
			SigningCustomerImgPath		= '/Content/img/status/sign_cust.png',	--Q5.ImageUrl,							-- Путь к изображению статуса подписания ЭП Клиентом

			ApprovalPerformerHint		= 'Согласована ЗАО «Русагротранс»; Согласована Клиентом',						-- Статус согласования Экспедитором
			ApprovalCustomerHint		= 'Согласована ЗАО «Русагротранс»; На согласовании Клиента',					-- Статус согласования Клиентом
			SigningPerformerHint		= 'Подписано ЭП Экспедитора',													-- Статус подписания ЭП Экспедитором
			SigningCustomerHint			= 'Подписано ЭП Клиента',														-- Статус подписания ЭП Клиентом

			TemplateName				= T.Name																		-- Шаблон

	FROM	dbo.Document D (nolock)
			INNER JOIN dbo.Template T (nolock) ON T.Id = D.TemplateId
			INNER JOIN NSI.Reference TT (nolock) ON TT.Id = T.TemplateTypeId AND (@Group IS NULL OR TT.CodeS = @Group)
			INNER JOIN dbo.Agreement A (nolock) ON A.DocumentId = D.Id
			INNER JOIN dbo.Company C_C (nolock) ON C_C.Id = D.CustomerCompanyId
			INNER JOIN dbo.Company C_P (nolock) ON C_P.Id = D.PerformerCompanyId
			LEFT JOIN dbo.vAgreementOwner AO ON AO.AgreementId = D.Id /*AND AO.CompanyId = @CompanyId*/ AND AO.RN = 1
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
			--OUTER APPLY
			--(
			--	SELECT	top 1 CASE WHEN DA6.Id IS NULL THEN NULL ELSE 'B42282D6-FC0A-4071-83A1-9BB413099885' END AS ImageUrl, DA6.FName
			--	FROM	dbo.DocumentAttach DA6 (nolock)
			--	WHERE	DA6.IsDeleted	= 0
			--		AND	DA6.DocumentId	= D.Id
			--) Q6
	WHERE	D.IsDeleted = 0
		AND	(
				D.CustomerCompanyId		= @CompanyId
			OR	D.PerformerCompanyId	= @CompanyId
			)
		AND	(
				@AgrManagerId IS NULL
			OR	AO.AgrOwnerId = @AgrManagerId
			)
		AND	(
				@TemplateId IS NULL
			OR	T.Id = @TemplateId
			)
		AND	(
				Coalesce(@OnlyActive, 0) = 0
			OR	GetDate() between Coalesce(A.DateBeg, '19900101') AND Coalesce(A.DateStop, A.DateEnd, '20500101') 
			)
/*---------------------------------------------------------------------------------------------------------------------------------------------------
 Version      Date    By    Changes  
 -------   --------   ---   -------------------------------------------------------------------------------------------------------------------------
 1.0       05/03/17   brv   Created
 -------   --------   ---   -------------------------------------------------------------------------------------------------------------------------
 P U T   Y O U R   C H A N G E   H I S T O R Y   A T    T H E   T O P !
---------------------------------------------------------------------------------------------------------------------------------------------------*/
END

GO
