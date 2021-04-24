USE ClientesGFT
GO

/* VALIDAÇÕES */

CREATE OR ALTER PROCEDURE SP_VerificarSeExisteUsuario
(
	@Id INTEGER
)
AS
BEGIN
	IF((SELECT COUNT(*) FROM Usuarios WHERE Id = @Id) <= 0)
		THROW 50001,'Usuário inexistente!' ,1;
END
GO

CREATE OR ALTER PROCEDURE SP_VerificarSeExisteDadosDoUsuario
(
	@Login VARCHAR(255),
	@IdUsuario INTEGER = NULL
)
AS
BEGIN
	IF(@IdUsuario IS NOT NULL)
		BEGIN
			IF((SELECT COUNT(*) FROM Usuarios WHERE Login = @Login AND Id <> @IdUsuario) > 0)
				THROW 50001,'Usuário já existe!' ,1;
		END
	ELSE
		IF((SELECT COUNT(*) FROM Usuarios WHERE Login = @Login) > 0)
				THROW 50001,'Usuário já existe!' ,1;
END
GO



CREATE OR ALTER PROCEDURE SP_ValidarUsuario
(
	@Nome VARCHAR(255),
	@Login VARCHAR(255),
	@Senha VARCHAR(255)
)
AS
BEGIN
	DECLARE @Erros VARCHAR(MAX)='';

	IF(@Nome IS NULL) OR (TRIM(@Nome) = '')
		SET @Erros += 'O Nome está inválido!' + CHAR(13);

	IF(@Login IS NULL) OR (TRIM(@Login) = '')
		SET @Erros += 'O Login está inválido!' + CHAR(13);

	IF(@Senha IS NULL) OR (TRIM(@Senha) = '')
		SET @Erros += 'A Senha está inválida!' + CHAR(13);

	IF(LEN(@Erros) > 0)
	BEGIN
		SET @Erros = TRIM(@Erros);
		THROW 50001 ,@Erros ,1;
	END
END
GO


/* PROCEDURES */

CREATE OR ALTER PROCEDURE SP_InserirUsuario
	@Nome VARCHAR(255),
	@Login VARCHAR(255),
	@Senha VARCHAR(255)
AS
	BEGIN TRY
		EXEC [dbo].[SP_ValidarUsuario] @Nome, @Login, @Senha;
		EXEC [dbo].[SP_VerificarSeExisteDadosDoUsuario] @Login;

		SET @Senha = TRIM(@Senha);
		EXEC [dbo].[SP_TransformarEmMD5] @Senha, @Senha OUTPUT;

		INSERT INTO Usuarios(Login,Nome,Senha) VALUES (TRIM(@Login), TRIM(@Nome), @Senha);
		SELECT SCOPE_IDENTITY() as Id_Inserido;
	END TRY
	BEGIN CATCH 
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
  END CATCH	
GO



CREATE OR ALTER PROCEDURE SP_AtualizarUsuario
	@Id INTEGER,
	@Nome VARCHAR(255) = NULL,
	@Login VARCHAR(255) = NULL,
	@Senha VARCHAR(255) = NULL
AS
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExisteUsuario] @Id;

		DECLARE @SenhaPreenchida BIT = IIF(@Senha IS NOT NULL, 1, 0);

		SELECT
			@Login = coalesce(@Login, Login),
			@Nome = coalesce(@Nome, Nome),
			@Senha = coalesce(@Senha, Senha)
		FROM Usuarios
		WHERE Id=@Id;

		EXEC [dbo].[SP_ValidarUsuario] @Nome, @Login, @Senha
		EXEC [dbo].[SP_VerificarSeExisteDadosDoUsuario] @Login, @Id;

		IF(@SenhaPreenchida = 1)
			SET @Senha = TRIM(@Senha);
			EXEC [dbo].[SP_TransformarEmMD5] @Senha, @Senha OUTPUT;
		
		UPDATE Usuarios 
		SET 
			Login = TRIM(@Login),
			Nome = TRIM(@Nome),
			Senha = @Senha
		WHERE Id=@Id;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_DeletarUsuario
	@Id INTEGER
AS
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExisteUsuario] @Id;
		UPDATE Usuarios SET Ativo=0 WHERE Id=@Id
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_Logar
	@Login VARCHAR(255),
	@Senha VARCHAR(255)
AS
	BEGIN TRY
		SET @Senha = TRIM(@Senha);
		EXEC [dbo].[SP_TransformarEmMD5] @Senha, @Senha OUTPUT;

		DECLARE @IdUsuario INTEGER =
									(SELECT Id FROM VW_Usuarios
										WHERE Login=TRIM(@Login) AND 
										Senha= @Senha
									 );
		IF(@IdUsuario IS NOT NULL) AND (@IdUsuario > 0)
			SELECT * FROM VW_Usuarios WHERE Id = @IdUsuario;
		ELSE
			THROW 50001,'Usuário e/ou senha inválido(s)!' ,1;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO
