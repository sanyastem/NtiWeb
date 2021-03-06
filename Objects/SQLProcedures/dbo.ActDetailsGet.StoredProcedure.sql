USE [SLP]
GO
/****** Object:  StoredProcedure [dbo].[ActDetailsGet]    Script Date: 17.03.2017 9:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ActDetailsGet] (
		@ActId uniqueidentifier		= NULL			--Id акта
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ServiceId			= NULL,					--Услуга							
		ServiceName			= 'Перевозка грузов',	--Услуга			
		NDS					= '18%',				--Ставка НДС				
		Summa				= 1093.22,				--Сумма без НДС			
		SummaNDS			= 1290.0 - 1093.22,		--Сумма НДС				
		SummaWithNDS		= 1290.0,				--Сумма с НДС					
		CurrencyName		= 'Российские рубли',	--Валюта			
		ConditionName		= NULL					--Параметры								

END

GO
