USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimRollingStockGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClaimRollingStockGet] (
			@ClaimId		uniqueidentifier,	-- Id заявки
			@CompanyId		uniqueidentifier	= '0AE1AA3E-022E-4510-9D9E-C55EADC89547',
			@ProtocolGroup	nvarchar(256)		= 'CLAIM-TEO'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ContParkName			= '',			--заглушка Парк контейнеров 							
		ContKindName			= '',			--заглушка Род контейнеров																																																																											
		CarParkName				= '',			--заглушка Парк вагонов
		CarKindName				= '',			--заглушка Род вагонов
		CarTypeName				= '',			--заглушка Тип вагонов																																																																	
		GroupTypeName			= '',			--заглушка Вид группы	
		ShipmentName			= ''			--заглушка Вид отправки																									

END

GO
