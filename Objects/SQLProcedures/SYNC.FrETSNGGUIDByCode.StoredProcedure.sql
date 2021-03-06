USE [SLP]
GO
/****** Object:  StoredProcedure [SYNC].[FrETSNGGUIDByCode]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [SYNC].[FrETSNGGUIDByCode]
	@Table	varchar(255),
	@GUID	varchar(255),
	@Code	varchar(255),
	@AddNew		bit = 0,
	@GegSync	bit = 1
AS
BEGIN
-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Заполнение поля идентификатора груза ЕТСНГ в таблице по коду груза ЕТСНГ ------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	SET NOCOUNT ON;

	DECLARE @S varchar(MAX)

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Зачистка значений, для которых есть ссылка на IsDeleted или IsAddBySync -------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT @S = 'UPDATE T SET T.' + @GUID + ' = NULL FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON T.' + @GUID + ' = F.Id WHERE F.IsDeleted = 1 OR IsAddBySync = 1'
	EXEC (@S)

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Идентификация груза ЕТСНГ по коду 6 -------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Грузы не отмеченные как удаленные, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6 AND F.IsDeleted = 0 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все шрузы, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все грузы
	IF @GegSync = 1
	BEGIN
		SELECT @S = 'UPDATE	T ' +
					'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
					'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
					'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6'
		EXEC (@S)
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Добавление станций с кодом 6 знаков -------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	IF @AddNew = 1
	BEGIN
--Добавление новых грузов с кодом 6 знаков
		SELECT @S = 'INSERT INTO NSI.FrETSNG(Code6, Name, IsDeleted, IsAddBySync) ' +
						'SELECT distinct ' + @Code + ', ''Груз ЕТСНГ с кодом "''+' + @Code + '+''"'', 1, 1 ' +
						'FROM ' + @Table + ' F (nolock) LEFT JOIN NSI.FrETSNG T (nolock) ON T.Code6 = F.' + @Code + ' ' +
						'WHERE F.' + @GUID + ' IS NULL AND T.Id IS NULL AND Len(Coalesce(F.' + @Code + ','''')) = 6'
		EXEC (@S)

--Повторная идентификация груза по коду 6 (после добавления новых грузов)
		IF @GegSync = 1
		BEGIN
			SELECT @S = 'UPDATE	T ' +
						'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
						'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code6 = T.' + @Code + ' ' +
						'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 6'
			EXEC (@S)
		END
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Идентификация станций по коду 5 -----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
-- Грузы не отмеченные как удаленные, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5 AND F.IsDeleted = 0 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все шрузы, кроме добавленных при синхронизации
	SELECT @S = 'UPDATE	T ' +
				'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
				'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
				'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5 AND F.IsAddBySync = 0'
	EXEC (@S)

-- Все грузы
	IF @GegSync = 1
	BEGIN
		SELECT @S = 'UPDATE	T ' +
					'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
					'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
					'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5'
		EXEC (@S)
	END

-----------------------------------------------------------------------------------------------------------------------------------------------------
--- Добавление станций с кодом 6 знаков -------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------
	IF @AddNew = 1
	BEGIN
--Добавление новых грузов с кодом 6 знаков
		SELECT @S = 'INSERT INTO NSI.FrETSNG(Code5, Name, IsDeleted, IsAddBySync) ' +
						'SELECT distinct ' + @Code + ', ''Груз ЕТСНГ с кодом "''+' + @Code + '+''"'', 1, 1 ' +
						'FROM ' + @Table + ' F (nolock) LEFT JOIN NSI.FrETSNG T (nolock) ON T.Code5 = F.' + @Code + ' ' +
						'WHERE F.' + @GUID + ' IS NULL AND T.Id IS NULL AND Len(Coalesce(F.' + @Code + ','''')) = 5'
		EXEC (@S)

--Повторная идентификация груза по коду 6 (после добавления новых грузов)
		IF @GegSync = 1
		BEGIN
			SELECT @S = 'UPDATE	T ' +
						'SET T.' + @GUID + ' = Coalesce(F.RealFrETSNGId, F.Id) ' +
						'FROM ' + @Table + ' AS T (nolock) INNER JOIN NSI.FrETSNG AS F (nolock) ON F.Code5 = T.' + @Code + ' ' +
						'WHERE T.' + @GUID + ' IS NULL AND Len(Coalesce(T.' + @Code + ','''')) = 5'
			EXEC (@S)
		END
	END
END

GO
