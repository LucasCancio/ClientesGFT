USE ClientesGFT
GO

/* VALIDAÇÕES */

CREATE OR ALTER PROCEDURE SP_VerificarSeExistePerfil
(
	@Id INTEGER
)
AS
BEGIN
	IF((SELECT COUNT(*) FROM Perfils WHERE Id = @Id) <= 0)
		THROW 50001,'Perfil inexistente!' ,1;
END
GO


/* PROCEDURES */

CREATE OR ALTER PROCEDURE SP_InserirPerfil
	@Nome VARCHAR(20)
AS
	BEGIN TRY
		IF(TRIM(ISNULL(@Nome,'')) = '')
			THROW 50001,'Perfil inválido!' ,1;

		INSERT INTO Perfils(Nome) VALUES (TRIM(@Nome));
		SELECT SCOPE_IDENTITY() as Id_Inserido;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_AtualizarPerfil
	@Id INTEGER,
	@Nome VARCHAR(20)
AS
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExistePerfil] @Id;

		IF(TRIM(ISNULL(@Nome,'')) = '')
			THROW 50001,'Perfil inválido!' ,1;

		UPDATE Perfils SET Nome=TRIM(@Nome) WHERE Id=@Id;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_DeletarPerfil
	@Id INTEGER
AS
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExistePerfil] @Id;

		UPDATE Perfils SET Ativo=0
		WHERE Id=@Id
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_AplicarPerfil
	@IdUsuario INTEGER,
	@IdPerfil INTEGER
AS
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExisteUsuario] @IdUsuario;
		EXEC [dbo].[SP_VerificarSeExistePerfil] @IdPerfil;

		INSERT INTO Usuario_Perfil (IdUsuario, IdPerfil)
		VALUES (@IdUsuario, @IdPerfil);
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_RetirarPerfil
	@IdUsuario INTEGER,
	@IdPerfil INTEGER
AS
	BEGIN TRY
		DELETE FROM Usuario_Perfil 
		WHERE IdUsuario = @IdUsuario AND IdPerfil = @IdPerfil;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO