USE ClientesGFT
GO

/* VALIDAÇÕES E FUNÇÕES */

CREATE OR ALTER PROCEDURE SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo
(
	@IdCliente INTEGER,
	@IdUsuario INTEGER
)
AS
BEGIN
	DECLARE 
		@IdStatus_EmCadastro INTEGER = 1,
		@IdStatus_CorrecaoPerfil INTEGER = 6;

	DECLARE @UltimoIdUsuario INTEGER = (SELECT TOP 1 IdUsuario FROM Fluxo_Aprovacao 
																	WHERE IdCliente = @IdCliente AND 
																	IdStatus NOT IN (@IdStatus_EmCadastro, @IdStatus_CorrecaoPerfil) 
																	ORDER BY Id DESC);

	IF(@UltimoIdUsuario = @IdUsuario)
		THROW 50001,'Usuário inválido para essa operação!' ,1;
END
GO


CREATE OR ALTER FUNCTION FC_Listar_IdsClientes_Indisponiveis
(
	@IdUsuario INTEGER
)
RETURNS  @resultTable TABLE 
(
    IdCliente INTEGER NOT NULL
)
AS
BEGIN
	DECLARE @IdStatus_EmCadastro INTEGER = 1;
	INSERT INTO @resultTable
		SELECT IdCliente FROM Fluxo_Aprovacao 
		WHERE Id IN (SELECT MAX(Id) FROM Fluxo_Aprovacao GROUP BY IdCliente)
		AND IdStatus <> @IdStatus_EmCadastro
		AND IdUsuario = @IdUsuario
RETURN
END
GO


/* FLUXO */


CREATE OR ALTER PROCEDURE SP_EnviarParaGerencia
(
	@IdCliente INTEGER,
	@IdUsuarioResponsavel INTEGER
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE 
				@IdStatus_EmCadastro INTEGER = 1,
				@IdStatus_CorrecaoPerfil INTEGER = 6,
				@IdStatus_AguardandoGerencia INTEGER = 2;
											  
		
		IF((SELECT COUNT(*) FROM Clientes 
								WHERE Id = @IdCliente AND 
								IdStatusAtual IN (@IdStatus_EmCadastro, @IdStatus_CorrecaoPerfil)) 
			<= 0)
			THROW 50001,'Cliente inválido para essa operação!' ,1;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'OPERACAO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		EXEC [dbo].[SP_VerificarSeExisteUsuario] @IdUsuarioResponsavel;

		UPDATE Clientes 
		SET IdStatusAtual=@IdStatus_AguardandoGerencia, DataAlteracao = GETDATE()
		WHERE Id=@IdCliente

		INSERT INTO Fluxo_Aprovacao
		(IdUsuario, IdCliente, IdStatus)
		VALUES (@IdUsuarioResponsavel, @IdCliente, @IdStatus_AguardandoGerencia)

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;

		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
END
GO



CREATE OR ALTER PROCEDURE SP_EnviarParaAprovacao
(
	@IdCliente INTEGER,
	@IdUsuarioResponsavel INTEGER
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE 
				@IdStatus_AguardandoGerencia INTEGER = 2,
				@IdStatus_AguardandoControleDeRisco INTEGER = 3;

		IF((SELECT COUNT(*) FROM Clientes 
								WHERE Id = @IdCliente AND 
								IdStatusAtual= @IdStatus_AguardandoGerencia) 
			<= 0)
			THROW 50001,'Cliente inválido para essa operação!' ,1;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'GERENCIA')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		EXEC [dbo].[SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo] @IdCliente,@IdUsuarioResponsavel;

		DECLARE 
				@ClienteInternacional BIT = 
						(SELECT ClienteInternacional FROM VW_Clientes WHERE Id=@IdCliente);
			
		IF(@ClienteInternacional = 1)
			BEGIN
				UPDATE Clientes 
				SET IdStatusAtual=@IdStatus_AguardandoControleDeRisco, DataAlteracao = GETDATE()
				WHERE Id=@IdCliente

				INSERT INTO Fluxo_Aprovacao
				(IdUsuario, IdCliente, IdStatus)
				VALUES (@IdUsuarioResponsavel, @IdCliente, @IdStatus_AguardandoControleDeRisco)
			END
		ELSE
			EXEC [dbo].[SP_AprovarCliente] @IdCliente, @IdUsuarioResponsavel;

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
END
GO



CREATE OR ALTER PROCEDURE SP_AprovarCliente
(
	@IdCliente INTEGER,
	@IdUsuarioResponsavel INTEGER
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE 
				@IdStatus_AguardandoGerencia INTEGER = 2,
				@IdStatus_AguardandoControleDeRisco INTEGER = 3,
				@IdStatus_Aprovado INTEGER = 4;

		IF((SELECT COUNT(*) FROM Clientes 
								WHERE Id = @IdCliente AND 
								IdStatusAtual IN 
									(@IdStatus_AguardandoGerencia, @IdStatus_AguardandoControleDeRisco)) 
			<= 0)
			THROW 50001,'Cliente inválido para essa operação!' ,1;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'GERENCIA', 'CONTROLE_DE_RISCO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		EXEC [dbo].[SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo] @IdCliente,@IdUsuarioResponsavel;

		UPDATE Clientes 
		SET IdStatusAtual=@IdStatus_Aprovado, DataAlteracao = GETDATE()
		WHERE Id=@IdCliente

		INSERT INTO Fluxo_Aprovacao
		(IdUsuario, IdCliente, IdStatus)
		VALUES (@IdUsuarioResponsavel, @IdCliente, @IdStatus_Aprovado)

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;

		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
END
GO



CREATE OR ALTER PROCEDURE SP_ReprovarCliente
(
	@IdCliente INTEGER,
	@IdUsuarioResponsavel INTEGER
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @IdStatus_Reprovado INTEGER = 5;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'GERENCIA', 'CONTROLE_DE_RISCO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;

		EXEC [dbo].[SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo] @IdCliente,@IdUsuarioResponsavel;

		UPDATE Clientes 
		SET IdStatusAtual=@IdStatus_Reprovado, DataAlteracao = GETDATE()
		WHERE Id=@IdCliente

		INSERT INTO Fluxo_Aprovacao
		(IdUsuario, IdCliente, IdStatus)
		VALUES (@IdUsuarioResponsavel, @IdCliente, @IdStatus_Reprovado)

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;

		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
END
GO



CREATE OR ALTER PROCEDURE SP_EnviarParaCorrecao
(
	@IdCliente INTEGER,
	@IdUsuarioResponsavel INTEGER
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @IdStatus_CorrecaoPerfil INTEGER = 6;

		IF((SELECT COUNT(*) FROM VW_Usuarios_Perfils 
								WHERE IdUsuario = @IdUsuarioResponsavel AND 
								Perfil in ('ADMINISTRACAO', 'GERENCIA', 'CONTROLE_DE_RISCO')) 
			<= 0)
			THROW 50001,'Usuário inválido para essa operação!' ,1;
		
		EXEC [dbo].[SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo] @IdCliente,@IdUsuarioResponsavel;

		UPDATE Clientes 
		SET IdStatusAtual=@IdStatus_CorrecaoPerfil, DataAlteracao = GETDATE()
		WHERE Id=@IdCliente

		INSERT INTO Fluxo_Aprovacao
		(IdUsuario, IdCliente, IdStatus)
		VALUES (@IdUsuarioResponsavel, @IdCliente, @IdStatus_CorrecaoPerfil)

		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;

		DECLARE @Message VARCHAR(MAX) = ERROR_MESSAGE(),
        @Severity INT = ERROR_SEVERITY(),
        @State SMALLINT = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH
END
GO