CREATE OR ALTER PROCEDURE SP_TransformarEmMD5
(
	@Texto VARCHAR(255),
	@TextoTransformado VARCHAR(255) OUTPUT 
)
AS
BEGIN
	SET @TextoTransformado = (SELECT HashBytes('MD5', @Texto));
END
GO