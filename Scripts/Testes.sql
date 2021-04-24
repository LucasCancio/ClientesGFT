/* USUÁRIOS */
USE ClientesGFT
GO
EXEC SP_InserirUsuario @Nome='', @Login='', @Senha='';

EXEC SP_InserirUsuario @Nome='Usuario 5', @Login='user5', @Senha='123';
EXEC SP_AtualizarUsuario @Id=4, @Login='risco';
EXEC SP_DeletarUsuario @Id=1;

EXEC SP_Logar @Login='operacao', @Senha='123'; /* Login */

SELECT * FROM Usuarios
SELECT * FROM VW_Usuarios;



/* PERFILS */
USE ClientesGFT
GO
EXEC SP_InserirPerfil @Nome='Perfil Teste';
EXEC SP_AtualizarPerfil @Id=1, @Nome='';
EXEC SP_DeletarPerfil @Id=1;

EXEC SP_AplicarPerfil @IdUsuario=8, @IdPerfil=1;
EXEC SP_RetirarPerfil @IdUsuario=7, @IdPerfil=2;

SELECT * FROM Perfils
SELECT * FROM VW_Usuarios_Perfils;



/* CLIENTES */
USE ClientesGFT
GO
EXEC SP_InserirCliente
@IdUsuarioResponsavel= 1, @Nome='Teste 1 Nacional', @CPF='743.084.220-86' , @DataNasc= '2000-12-03', @Email='teste1@gmail.com',
@IdCidade=1, @Rua='Alameda Rio Negro', @Bairro='Alphaville Industrial', @Numero=2,
@Cep='06454-000', @RG='16.900.621-9';

EXEC SP_InserirCliente
@IdUsuarioResponsavel= 1, @Nome='Teste 2 Internacional', @CPF='382.822.360-55' , @DataNasc= '2003-02-06', @Email='teste2@gmail.com',
@IdCidade=3, @Rua='Rua Florida', @Bairro='Centro', @Numero=201, @Complemento='Predio 2';

EXEC SP_InserirCliente
@IdUsuarioResponsavel= 1, @Nome='Teste 3 Nacional', @CPF='539.977.890-45' , @DataNasc= '1992-12-01', @Email='teste3@gmail.com',
@IdCidade=2, @Rua='Rua 13 de Maio', @Bairro='Cantinho do Céu', @Numero=1,
@Cep='04849529', @RG='46.203.550-5';

EXEC SP_InserirCliente
@IdUsuarioResponsavel= 1, @Nome='Teste 4 Internacional', @CPF='500.017.760-61' , @DataNasc= '2003-02-06', @Email='teste4@gmail.com',
@IdCidade=4, @Rua='Rua Florida', @Bairro='Centro', @Numero=104;

EXEC SP_InserirCliente
@IdUsuarioResponsavel= 1, @Nome='', @CPF=NULL , @DataNasc= NULL, @Email='teste.com',
@IdCidade=4, @Rua='Rua Florida', @Bairro='Centro', @Numero=104;

EXEC SP_AtualizarCliente
@Id=5, @Email='testado10@gmail.com', @Nome='Testado 10 Nacional';

EXEC SP_DeletarCliente @IdUsuarioResponsavel=1, @IdCliente=34;

SELECT * FROM Clientes;
SELECT * FROM VW_Clientes;

DECLARE @list AS [TelefoneList]

INSERT INTO @list VALUES (0,'(11) 23123-2323')
INSERT INTO @list VALUES (38,'(12) 23123-2323')
INSERT INTO @list VALUES (40,'(21) 3123-2323')

EXEC [dbo].[SP_AtualizarTelefones] 27, @list

SELECT * FROM VW_Clientes_Telefones;



/* ENDEREÇO */
USE ClientesGFT
GO
SELECT * FROM Enderecos;
SELECT * FROM VW_Enderecos;



/* FLUXO DE APROVAÇÃO */

SELECT * FROM VW_Fluxo_Aprovacao;

/* Quando a Operação enviar para aprovação */
EXEC SP_EnviarParaGerencia @IdCliente=2,@IdUsuarioResponsavel=1; 

/* Quando a Gerência aprovar o cadastro:
		*Se o cliente é Internacional -> Atualiza o status para 'Aguardando aprovação de Controle de Risco' 
		*Se o cliente é Nacional -> Envia para SP_AprovarCliente */
EXEC SP_EnviarParaAprovacao @IdCliente=2,@IdUsuarioResponsavel=1; 

/* Cliente nacional -> Vem da SP_EnviarParaAprovacao
   Cliente internaconal -> Quando o controle de risco aprovar */
EXEC SP_AprovarCliente @IdCliente=2,@IdUsuarioResponsavel=1; 
									
/* Quando alguém reprovar o cadastro */
EXEC SP_ReprovarCliente @IdCliente=6,@IdUsuarioResponsavel=1; 

/* Quando alguém enviar o cadastro para correção */
EXEC SP_EnviarParaCorrecao @IdCliente=6,@IdUsuarioResponsavel=1; 


SELECT * FROM [dbo].[FC_Listar_IdsClientes_Indisponiveis](1);

DECLARE @Teste VARCHAR(MAX);
EXEC [dbo].[SP_TransformarEmMD5] '123', @Teste OUTPUT;
SELECT @Teste