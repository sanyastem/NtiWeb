USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[CheckDBContentRun]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckDBContentRun]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Recipients varchar(255) = 'boytsun_rv@siam-consult.ru'
	DECLARE @HTML nvarchar(max)

	SET @HTML =	'<HTML>
				<HEAD><style type="text/css"> body { font-size: 100%; font-family: Arial, Helvetica, sans-serif; } tr { font-size: 85%; } </style> </HEAD>
				<BODY><H3>Контроль полноты заполнения данными объектов АСУ ВП (ULP)</H3>Данное письмо отправлено автоматически и не требует ответа.<BR>
				<TABLE><table border="1"><TR><th>Группа данных</th><th>Проблема</th><th>Элементов</th><th>Запрос</th></TR>'

	DECLARE @S nvarchar(MAX), @P nvarchar(20), @ValuesCount integer

	DECLARE @Query nvarchar(MAX), @SortGroup nvarchar(255), @Comment nvarchar(255)
	DECLARE cursorCheckDBContent CURSOR FOR 
		SELECT	Query, SortGroup, Comment
		FROM	dbo.CheckDBContent
		ORDER BY SortOrder
	OPEN cursorCheckDBContent
	FETCH NEXT FROM cursorCheckDBContent INTO @Query, @SortGroup, @Comment
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @S = 'SELECT @ValuesCount = Count(*) FROM ('+@Query+') S'
		SET	@P = '@ValuesCount int output'
		EXEC sp_executesql @S, @P, @ValuesCount OUT

		IF @ValuesCount > 0
		BEGIN
			SET @HTML = @HTML + '<TR><th>'+@SortGroup+'</th><td>'+@Comment+'</td><th>'+convert(varchar,@ValuesCount)+'</th><td>'+@Query+'</td></TR>'
		END

		FETCH NEXT FROM cursorCheckDBContent INTO @Query, @SortGroup, @Comment
	END 
	CLOSE cursorCheckDBContent
	DEALLOCATE cursorCheckDBContent

	SET @HTML = @HTML + '</TABLE></BODY></HTML>'

	DECLARE @SendEmailReturnCode INT = NULL;
	EXEC	@SendEmailReturnCode = msdb.dbo.sp_send_dbmail
			@profile_name			= 'Sql DataBasemail',
			@recipients             = @Recipients,
			@subject                = 'CONTROLLER. Контроль загрузчиков данных АСУ ВП',
			@body_format            = 'HTML',
			@body					= @HTML
END

GO
