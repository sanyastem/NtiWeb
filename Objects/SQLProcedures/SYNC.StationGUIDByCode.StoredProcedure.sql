USE [SLP]
GO
/****** Object:  StoredProcedure [SYNC].[StationGUIDByCode]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [SYNC].[StationGUIDByCode]
	@Table		nvarchar(255),
	@GUID		nvarchar(255),
	@Code		nvarchar(255),
	@AddNew		bit = 0,
	@GegSync	bit = 1
AS
BEGIN
-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Заполнение поля идентификатора станции в таблице по коду станции --------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	SET NOCOUNT ON;

	DECLARE @S varchar(max)

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Зачистка значений, для которых есть ссылка на IsDeleted или IsAddBySync -------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT @S = 'UPDATE T SET T.' + @GUID + ' = NULL FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON T.' + @GUID + ' = F.Id WHERE F.IsDeleted = 1 OR IsAddBySync = 1'
	EXEC (@S)

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Идентификация станций по коду 6 -----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Станции не отмеченные как удаленные, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6 AND F.IsDeleted = 0 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все станции, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все станции
	IF @GegSync = 1
	BEGIN
		SELECT @S = 'UPDATE	T ' +
					'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
					'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
					'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6'
		EXEC (@S)
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Добавление станций с кодом 6 знаков -------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	IF @AddNew = 1
	BEGIN
--Добавление новых станций с кодом 6 знаков
		SELECT @S = 'INSERT INTO NSI.Station(Code6, Name, IsDeleted, IsAddBySync) ' +
						'SELECT distinct F.' + @Code + ', ''Станция с кодом "'' + F.' + @Code + '+''"'', 1, 1 ' +
						'FROM ' + @Table + ' F (nolock) LEFT JOIN NSI.Station T (nolock) ON T.Code6 = F.' + @Code + ' ' +
						'WHERE F.' + @GUID + ' IS NULL AND T.StationGUID IS NULL AND Len(Coalesce(F.' + @Code + ','''')) = 6'
		EXEC (@S)

--Повторная идентификация станций по коду 6 (после добавления новых станций)
		IF @GegSync = 1
		BEGIN
			SELECT @S = 'UPDATE	T ' +
						'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
						'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
						'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6'
			EXEC (@S)
		END
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Идентификация станций по коду 5 -----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Станции не отмеченные как удаленные, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5 AND F.IsDeleted = 0 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все станции, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все станции
	IF @GegSync = 1
	BEGIN
		SELECT @S = 'UPDATE	T ' +
					'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
					'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
					'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5'
		EXEC (@S)
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Добавление станций с кодом 5 знаков -------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	IF @AddNew = 1
	BEGIN
--Добавление новых станций с кодом 5 знаков
			SELECT @S = 'INSERT INTO NSI.Station(Code5, Name, IsDeleted, IsAddBySync) ' +
						'SELECT distinct ' + @Code + ', ''Станция с кодом "'' + F.' + @Code + '+''"'', 1, 1 ' +
						'FROM ' + @Table + ' F (nolock) LEFT JOIN NSI.Station T (nolock) ON T.Code5 = F.' + @Code + ' ' +
						'WHERE F.' + @GUID + ' IS NULL AND T.StationGUID IS NULL AND Len(Coalesce(F.' + @Code + ','''')) = 5'
			EXEC (@S)

--Повторная идентификация станций по коду 5 (после добавления новых станций)
		IF @GegSync = 1
		BEGIN
			SELECT @S = 'UPDATE	T ' +
						'SET T.' + @GUID + ' = Coalesce(F.RealStationId, F.Id) ' +
						'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.Station AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
						'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5'
			EXEC (@S)
		END
	END
END

GO
