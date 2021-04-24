USE ClientesGFT
GO

/* VALIDAÇÕES */

CREATE OR ALTER PROCEDURE SP_VerificarSeExisteCliente
(
	@Id INTEGER
)
AS
BEGIN
	IF((SELECT COUNT(*) FROM Clientes WHERE Id = @Id) <= 0)
		THROW 50001,'Cliente inexistente!' ,1;
END
GO


CREATE OR ALTER PROCEDURE SP_VerificarSeExisteDadosDoCliente
(
	@CPF VARCHAR(11),
	@IdCliente INTEGER = NULL
)
AS
BEGIN
	IF(@IdCliente IS NOT NULL)
		BEGIN
			IF((SELECT COUNT(*) FROM Clientes WHERE CPF = @CPF AND Id <> @IdCliente) > 0)
				THROW 50001,'CPF já está em uso.' ,1;
		END
	ELSE
		IF((SELECT COUNT(*) FROM Clientes WHERE CPF = @CPF) > 0)
				THROW 50001,'Cliente já existe.' ,1;
END
GO

CREATE OR ALTER PROCEDURE SP_ValidarCliente
(
	@Nome VARCHAR(255),
	@CPF VARCHAR(11),
	@DataNasc DATETIME,
	@Email VARCHAR(255),
	
	@IdCidade INTEGER,
	@Rua VARCHAR(255),
	@Bairro VARCHAR(255),
	@Numero INT
)
AS
BEGIN
	DECLARE 
			@Erros VARCHAR(MAX) = '',
			@ClienteInternacional BIT;
	
	IF((SELECT COUNT(*) FROM Cidades WHERE Id=@IdCidade) <= 0)
		SET @Erros += 'A cidade está inválida!' + CHAR(13);

	SET @ClienteInternacional = IIF((SELECT Pais FROM VW_Enderecos WHERE IdCidade=@IdCidade) <> 'Brasil',1,0);

	IF(@Nome IS NULL) OR (TRIM(@Nome) = '')
		SET @Erros += 'O Nome está inválido!' + CHAR(13);

	IF	(@CPF IS NULL) OR
		((@ClienteInternacional = 0) AND ((SELECT [dbo].[FC_ValidarCPF](@CPF)) = 0))
		SET @Erros += 'O CPF está inválido!' + CHAR(13);

	IF(@DataNasc IS NULL) OR (YEAR(GETDATE()) <= YEAR(@DataNasc))
		SET @Erros += 'A Data de Nascimento está inválida!' + CHAR(13);

	IF(@Email IS NULL) OR ((SELECT [dbo].[FC_ValidarEmail](@Email)) = 0)
		SET @Erros += 'O Email está inválido!' + CHAR(13);

	IF(@Rua IS NULL) OR (TRIM(@Rua) = '')
		SET @Erros += 'A Rua está inválida!' + CHAR(13);

	IF(@Bairro IS NULL) OR (TRIM(@Bairro) = '')
		SET @Erros += 'O Bairro está inválido!' + CHAR(13);

	IF(@Numero IS NULL) OR (@Numero <= 0)
		SET @Erros += 'O Número está inválido!' + CHAR(13);

	IF(LEN(@Erros) > 0)
	BEGIN
		SET @Erros = TRIM(@Erros);
		THROW 50001 ,@Erros ,1;
	END
END
GO



/* PROCEDURES */

CREATE OR ALTER PROCEDURE SP_InserirCliente
	@IdUsuarioResponsavel INTEGER,
	@Nome VARCHAR(255),
	@CPF VARCHAR(14),
	@DataNasc DATETIME,
	@Email VARCHAR(255),
	
	@IdCidade INTEGER,
	@Rua VARCHAR(255),
	@Bairro VARCHAR(255),
	@Numero INT,
	@Complemento VARCHAR(255) = NULL,

	/* Somente para Nacional*/
	@Cep VARCHAR(9) = NULL,
	@RG VARCHAR(12) = NULL
AS
	BEGIN TRAN
	BEGIN TRY
		SET @CPF = (SELECT [dbo].[FC_FormatarDocumento](@CPF));
		SET @RG = (SELECT [dbo].[FC_FormatarDocumento](@RG));
		SET @Cep = (SELECT [dbo].[FC_FormatarDocumento](@Cep));

		EXEC [dbo].[SP_ValidarCliente] @Nome, @CPF, @DataNasc, @Email, @IdCidade, @Rua, @Bairro, @Numero;
		EXEC [dbo].[SP_VerificarSeExisteDadosDoCliente] @CPF;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'OPERACAO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		INSERT INTO Enderecos(Cep,Complemento,Numero,IdCidade, Bairro, Rua)
		VALUES(@Cep, TRIM(@Complemento), @Numero, @IdCidade, @Bairro, @Rua);

		DECLARE @IdStatus_EmCadastro INTEGER = 1;

		INSERT INTO Clientes(Nome ,CPF ,RG ,DataNasc, Email, IdEndereco, IdStatusAtual)
		VALUES (TRIM(@Nome) ,@CPF ,@RG ,@DataNasc, TRIM(@Email), 
				(SELECT TOP(1) Id FROM Enderecos ORDER BY Id DESC),
				@IdStatus_EmCadastro);

		DECLARE @IdInserido INTEGER = (SELECT SCOPE_IDENTITY());

		INSERT INTO Fluxo_Aprovacao
		(IdUsuario, IdCliente, IdStatus)
		VALUES (@IdUsuarioResponsavel, @IdInserido, @IdStatus_EmCadastro)

		SELECT @IdInserido as IdInserido;

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_AtualizarCliente
	@Id INTEGER,
	@Nome VARCHAR(255) = NULL,
	@CPF VARCHAR(14) = NULL,
	@DataNasc DATETIME = NULL,
	@Email VARCHAR(255) = NULL,
	
	@IdCidade INTEGER = NULL,
	@Rua VARCHAR(255) = NULL,
	@Bairro VARCHAR(255) = NULL,
	@Numero INT = NULL,
	@Complemento VARCHAR(255) = NULL,

	/* Somente para Nacional*/
	@Cep VARCHAR(9) = NULL,
	@RG VARCHAR(12) = NULL
AS
	BEGIN TRAN
	BEGIN TRY
		DECLARE 
				@IdStatus_EmCadastro INTEGER = 1,
				@IdStatus_CorrecaoPerfil INTEGER = 6;

		IF((SELECT COUNT(*) FROM Clientes 
								WHERE Id = @Id AND 
								IdStatusAtual in (@IdStatus_EmCadastro,@IdStatus_CorrecaoPerfil)) 
			<= 0)
			THROW 50001,'Cliente inválido para essa operação!' ,1;

		SELECT 
			@Nome = ISNULL(@Nome,cli.Nome), 
			@CPF = ISNULL(@CPF,cli.CPF),
			@DataNasc = ISNULL(@DataNasc, cli.DataNasc),
			@RG = ISNULL(@RG,cli.RG),
			@Email = ISNULL(@Email,cli.Nome),
			@IdCidade = ISNULL(@IdCidade,cd.Id),
			@Rua = ISNULL(@Rua, ed.Rua),
			@Bairro = ISNULL(@Bairro, ed.Bairro),
			@Cep = ISNULL(@Cep,ed.Cep),
			@Numero = ISNULL(@Numero,ed.Numero),
			@Complemento = ISNULL(@Complemento,ed.Complemento)
		FROM Clientes cli
		INNER JOIN Enderecos ed on cli.IdEndereco = ed.Id
		INNER JOIN Cidades cd on ed.IdCidade = cd.Id
		WHERE cli.Id = @Id

		SET @CPF = (SELECT [dbo].[FC_FormatarDocumento](@CPF));
		SET @RG = (SELECT [dbo].[FC_FormatarDocumento](@RG));
		SET @Cep = (SELECT [dbo].[FC_FormatarDocumento](@Cep));

		EXEC [dbo].[SP_ValidarCliente] @Nome, @CPF, @DataNasc, @Email, @IdCidade, @Rua, @Bairro, @Numero;
		EXEC [dbo].[SP_VerificarSeExisteDadosDoCliente] @CPF, @Id;

		DECLARE @IdEndereco INTEGER = ( SELECT en.Id 
										FROM Enderecos en 
										INNER JOIN Clientes cli on cli.IdEndereco = en.Id
										WHERE cli.Id=@Id );

		UPDATE Clientes
		SET 
			Nome = @Nome,
			CPF = @CPF,
			RG = @RG,
			DataNasc = @DataNasc,
			Email = @Email,
			DataAlteracao = GETDATE()
		WHERE Id = @Id;

		UPDATE Enderecos
		SET
			Cep = @Cep,
			IdCidade = @IdCidade,
			Bairro = @Bairro,
			Rua = @Rua,
			Numero = @Numero,
			Complemento = @Complemento
		WHERE Id = @IdEndereco

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_DeletarCliente
	@IdUsuarioResponsavel INTEGER,
	@IdCliente INTEGER
AS
	BEGIN TRAN
	BEGIN TRY
		DECLARE @IdStatus_EmCadastro INTEGER = 1;

		IF((SELECT COUNT(*) FROM Clientes 
								WHERE Id = @IdCliente AND 
								IdStatusAtual IN (@IdStatus_EmCadastro)) 
			<= 0)
			THROW 50001,'Cliente inválido para essa operação!' ,1;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'OPERACAO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		DELETE FROM Fluxo_Aprovacao WHERE IdCliente = @IdCliente;
		DELETE FROM Clientes_Telefones WHERE IdCliente = @IdCliente;
		DELETE FROM Clientes WHERE Id = @IdCliente;

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE TYPE [dbo].[TelefoneList] AS TABLE(
	[Id] [INTEGER],
    [Telefone] [VARCHAR](20) NOT NULL
);
GO

CREATE OR ALTER PROCEDURE SP_InserirTelefones
	@Telefones TelefoneList READONLY
AS
	BEGIN TRY
		DECLARE @IdCliente INTEGER = (SELECT MAX(Id) FROM Clientes);

		INSERT INTO Clientes_Telefones(IdCliente, Telefone)
		SELECT @IdCliente, Telefone FROM @Telefones;
	END TRY
	BEGIN CATCH
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO



CREATE OR ALTER PROCEDURE SP_AtualizarTelefones
	@IdCliente INTEGER,
	@Telefones TelefoneList READONLY
AS
	BEGIN TRAN
	BEGIN TRY
		EXEC [dbo].[SP_VerificarSeExisteCliente] @IdCliente;

		DECLARE 
				@TelefonesNoBanco TelefoneList,
				@TelefonesParaDeletar TelefoneList,
				@TelefonesParaInserir TelefoneList;

		INSERT INTO @TelefonesNoBanco
		SELECT Id, Telefone 
		FROM Clientes_Telefones WHERE IdCliente = @IdCliente

		INSERT INTO @TelefonesParaDeletar
		SELECT Id, Telefone FROM @TelefonesNoBanco
		WHERE Id NOT IN (SELECT Id FROM @Telefones);

		INSERT INTO @TelefonesParaInserir
		SELECT Id, Telefone FROM @Telefones
		WHERE Id = 0;

		INSERT INTO Clientes_Telefones(IdCliente, Telefone)
		SELECT @IdCliente, Telefone FROM @TelefonesParaInserir;

		DELETE FROM Clientes_Telefones
		WHERE IdCliente = @IdCliente AND Id IN (SELECT Id from @TelefonesParaDeletar);

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
GO