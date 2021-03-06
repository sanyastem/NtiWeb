USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ProtocolDetailsGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ProtocolDetailsGet] 
	@ProtocolId				uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	ServiceId				= SL.Id,
			ServiceName				= Coalesce(LTrim(RTrim(SUBSTRING(SC.Note, 16, CHARINDEX ('!' , SC.Note, 16) - 16))), SL.Name),									-- Услуга
			MeasureName				= R_M.ShortName,																												-- Единица измерения
			NDS						= CASE WHEN SC.NDS IS NULL then 'Не облагается' ELSE CAST(CAST(100 * SC.NDS AS integer) AS nvarchar) + ' %' END,				-- Ставка НДС
			Summa					= SC.Summa,																														-- Сумма без НДС
			SummaNDS				= SC.SummaNDS,																													-- Сумма НДС
			SummaWithNDS			= SC.SummaWithNDS,																												-- Сумма с НДС
			CurrencyName			= cur.Name,																														-- Валюта
			CountryName				= cou.Name,																														-- Территория
			StationFromName			= stf.Code6 + '//' + stf.Name + ' ('+ stf.RailWayShortName +')',																-- Станция отправления
			StationToName			= stt.Code6 + '//' + stt.Name + ' ('+ stt.RailWayShortName +')',																-- Станция назначения
			FreightName				= '',																															-- Груз
			GroupTypeName			= rgt.Name,
			CarParkName				= CP.Name
	FROM	dbo.ServiceCost SC (nolock)
			INNER JOIN NSI.ServiceList SL (nolock) ON SL.Id = SC.ServiceId
			INNER JOIN NSI.Reference R_M (nolock) ON R_M.Id = SC.MeasureId
			INNER JOIN NSI.Currency cur (nolock) ON cur.Id = SC.CurrencyId
			LEFT JOIN NSI.Country cou (nolock) ON cou.Id = SC.CountryId
			LEFT JOIN NSI.vStationExt stf (nolock) ON stf.Id = SC.StationFromId
			LEFT JOIN NSI.vStationExt stt (nolock) ON stt.Id = SC.StationToId
			LEFT JOIN NSI.Reference rgt (nolock) ON rgt.Id = SC.GroupTypeId
			LEFT JOIN dbo.CarPark CP (nolock) ON CP.Id = SC.CarParkId
	WHERE	SC.DocumentId = @ProtocolId
END

GO
