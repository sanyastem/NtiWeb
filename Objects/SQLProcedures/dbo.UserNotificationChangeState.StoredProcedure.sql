USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[UserNotificationChangeState]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UserNotificationChangeState] (
		@NotificationId		 uniqueidentifier,				-- Id оповещения
		@UpdatedBy			 uniqueidentifier				-- Id просмотревшего уведомление
	)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE  @ViewDate datetime = GetDate(); 
	DECLARE  @minDate datetime = CAST('1900-01-01 00:00:00.000' as datetime)

	
	UPDATE dbo.UserNotification SET 
	ViewDate = @ViewDate,
	UpdatedBy = @UpdatedBy
	WHERE Id = @NotificationId AND ViewDate = @minDate

END

GO
