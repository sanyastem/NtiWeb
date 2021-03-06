USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ClaimRouteDetailsGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClaimRouteDetailsGet]
				@StationFromId		uniqueidentifier	= 'EF048884-923F-4678-BC34-FD4FAFA32F5D',	-- станция отправления по Заявке
				@StationToId		uniqueidentifier	= '8335ACB5-6AF0-4C81-912E-58404FF64EAA',	-- станция назначения по Заявке
				@Options			nvarchar(max)		= NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	RouteId					= RD.RouteId,											
			Position				= RD.Position, 
			CountryName				= C.ShortName,					-- Страна
			StationInCode			= SF.Code6,						-- Станция входа (код)									
			StationInName			= SF.Name,						-- Станция входа					 
			RailWayInName			= SF.RailWayShortName, 			-- Дорога входа					
			StationOutCode			= ST.Code6, 					-- Станция выхода (код)							
			StationOutName			= ST.Name, 						-- Станция выхода						
			RailWayOutName			= ST.RailWayShortName, 			-- Дорога входа					
			Distance				= RD.Distance					-- Расстояние (км)					

	FROM	NSI.RouteInfo RI (nolock)
			INNER JOIN NSI.RouteDetail RD (nolock) ON RD.RouteId = RI.Id
			INNER JOIN NSI.Country C (nolock) ON C.Id = RD.CountryId
			INNER JOIN NSI.vStationExt SF (nolock) ON SF.Id = RD.StationBegId
			INNER JOIN NSI.vStationExt ST (nolock) ON ST.Id = RD.StationEndId

	WHERE	RI.StationFromId			= @StationFromId
			AND RI.StationToId			= @StationToId
			AND Coalesce(RI.Options,'')	= Coalesce(@Options,'')

	ORDER BY RD.Position
END
GO
