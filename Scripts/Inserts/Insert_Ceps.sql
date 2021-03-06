USE [ClientesGFT]
GO

/* PAISES */
INSERT Paises ([Id], [Pais], [Sigla]) VALUES (1, 'Brasil', 'BR')
INSERT Paises ([Id], [Pais], [Sigla]) VALUES (2, 'Argentina', 'AR')
INSERT Paises ([Id], [Pais], [Sigla]) VALUES (3, 'Estados Unidos', 'US')

/* ESTADOS */
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (1, 'São Paulo', 1)
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (2, 'Rio de Janeiro', 1)
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (3, 'Buenos Aires', 2)
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (4, 'California', 3)
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (5, 'Texas', 3)
INSERT [dbo].[Estados] ([Id], [Estado], [IdPais]) VALUES (6, 'New York', 3)

/* CIDADES */
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (1, 'São Paulo', 1)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (2, 'Barueri', 1)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (3, 'Rio de Janeiro', 2)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (4, 'Buenos Aires', 3)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (5, 'Los Angeles', 4)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (6, 'San Diego', 4)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (7, 'São Francisco', 4)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (8, 'Houston', 5)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (9, 'Dallas', 5)
INSERT [dbo].[Cidades] ([Id], [Cidade], [IdEstado]) VALUES (10, 'New York', 6)

/* CEPS */
/*
INSERT [dbo].[Ceps] ([Cep], [Rua], [Bairro], [IdCidade]) VALUES ('06454000', 'Alameda Rio Negro', 'Alphaville Industrial', 2)
INSERT [dbo].[Ceps] ([Cep], [Rua], [Bairro], [IdCidade]) VALUES ('04849529', 'Rua 13 de Maio', 'Cantinho do Céu', 1)*/
