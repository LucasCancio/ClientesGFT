USE ClientesGFT;
GO

CREATE OR ALTER VIEW VW_Enderecos AS
SELECT
	  c.Id as IdCidade,	
	  c.Cidade,
	  e.Id as IdEstado,	
	  e.Estado,
	  p.Id as IdPais,	
	  p.Pais
FROM Cidades c
INNER JOIN Estados e on e.Id = c.IdEstado
INNER JOIN Paises p on p.Id = e.IdPais
GO


CREATE OR ALTER VIEW VW_Usuarios AS
SELECT
	u.*
FROM Usuarios u
WHERE u.Ativo = 1
GO


CREATE OR ALTER VIEW VW_Usuarios_Perfils AS
SELECT
	u.Id as IdUsuario,
	u.Nome,
	p.Id as IdPerfil,
	p.Nome as Perfil
FROM Usuario_Perfil up
INNER JOIN Usuarios u on (u.Id = up.IdUsuario AND u.Ativo = 1)
INNER JOIN Perfils p on p.Id = up.IdPerfil
GO



CREATE OR ALTER VIEW VW_Clientes AS
SELECT
	cli.Id,
	IIF(pa.Pais <> 'Brasil', 1,0) as ClienteInternacional,
	cli.Nome,
	cli.CPF,
	cli.RG,
	cli.DataNasc,
	cli.Email,
	cli.IdStatusAtual,
	sf.Descricao as StatusAtual,
	ed.Cep,
	ed.Bairro,
	ed.Rua,
	cd.Id as IdCidade,
	cd.Cidade,
	es.Id as IdEstado,
	es.Estado,
	pa.Id as IdPais,
	pa.Sigla,
	pa.Pais,
	ed.Numero,
	ed.Complemento,
	cli.DataAlteracao
FROM Clientes cli
INNER JOIN Status_Fluxo sf on cli.IdStatusAtual = sf.Id
INNER JOIN Enderecos ed on cli.IdEndereco = ed.Id
INNER JOIN Cidades cd on ed.IdCidade = cd.Id
INNER JOIN Estados es on cd.IdEstado = es.Id
INNER JOIN Paises pa on es.IdPais = pa.Id
GO

CREATE OR ALTER VIEW VW_Clientes_Telefones AS
SELECT 
	c.Nome,
	ct.Telefone
FROM Clientes_Telefones ct
INNER JOIN Clientes c on c.Id = ct.IdCliente
GO



CREATE OR ALTER VIEW VW_Fluxo_Aprovacao AS
SELECT
	fa.Id,
	u.Id as IdUsuario,
	u.Nome as Usuario,
	c.Id as IdCliente,
	c.Nome as Cliente,
	c.CPF,
	st.Id as IdStatus,
	st.Descricao as Status,
	fa.DataCriacao
FROM Fluxo_Aprovacao fa
INNER JOIN Usuarios u on fa.IdUsuario = u.Id
INNER JOIN Clientes c on fa.IdCliente = c.Id
INNER JOIN Status_Fluxo st on fa.IdStatus = st.Id
GO
