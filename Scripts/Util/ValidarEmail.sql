USE [ClientesGFT]
GO
CREATE OR ALTER FUNCTION [dbo].[FC_ValidarEmail](@Ds_Email varchar(max))
RETURNS BIT
AS BEGIN
 
    DECLARE @Retorno BIT = 0

	IF(TRIM(@Ds_Email) = '') RETURN 0;
	IF(@Ds_Email IS NULL) RETURN 1;

    SELECT @Retorno = 1
    WHERE 
		@Ds_Email NOT LIKE '%[^a-z,0-9,@,.,_,-]%' AND 
		@Ds_Email LIKE '%_@_%_.__%'	AND 
		@Ds_Email NOT LIKE '%_@@_%_.__%'
 
    RETURN @Retorno
 
END