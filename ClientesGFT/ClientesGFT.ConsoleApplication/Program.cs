using ClientesGFT.Data.Repositories;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var usuario = Login();
                List<Opcao> opcoes = AplicarOpcoes(usuario);

                var userRepository = new UserSQLRepository(new RoleSQLRepository());
                var clientRepository = new ClientSQLRepository();
                var fluxoRepository = new FluxoSQLRepository(userRepository, clientRepository);

                var clienteService = new ClientService(clientRepository , fluxoRepository, userRepository);
                var fluxoService = new FluxoDeAprovacaoService(fluxoRepository, new ClientSQLRepository(), userRepository);


                while (true)
                {
                    try
                    {
                        Rotinas(usuario, opcoes, clienteService, fluxoService);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();

                        Console.WriteLine("Deseja continuar? se não, digite n");
                        if (Console.ReadLine().Trim().ToLower() == "n")
                            Environment.Exit(0);

                        Console.Clear();
                    }
                }



            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                Console.ResetColor();
            }

        }

        private static User Login()
        {
            User usuario;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--- Bem vindo(a) ao programa ClientesGFT ---");
                Console.ResetColor();

                Console.WriteLine("Para continuar, por favor faça o login. \n");

                Console.WriteLine("Digite seu usuario: ");
                string username = Console.ReadLine();
                Console.WriteLine("Digite sua senha: ");
                string senha = Console.ReadLine();

                var roleRepository = new RoleSQLRepository();
                var login = new UserService(new UserSQLRepository(roleRepository), roleRepository);

                usuario = login.Login(username, senha);

                if (usuario == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nUsuário e/ou senha estão inválidos!");
                    Console.ResetColor();

                    Console.WriteLine("Deseja continuar? se não, digite n");
                    if (Console.ReadLine().Trim().ToLower() == "n")
                        Environment.Exit(0);
                }

                Console.Clear();

            } while (usuario == null);

            return usuario;
        }

        private static List<Opcao> AplicarOpcoes(User usuario)
        {
            var opcoes = new List<Opcao>
            {
                new Opcao(Acoes.CONSULTAR_CLIENTE, "Consultar Cliente")
            };


            if (usuario.Roles.Contains(ERoles.ADMINISTRACAO))
            {
                opcoes
                      .AplicarAcoesExclusivasOperacao()
                      .AplicarAcoesExclusivasAdministracao()
                      .AplicarAcoesReprovarECorrecao();
            }
            else
            {
                if (usuario.Roles.Contains(ERoles.OPERACAO))
                    opcoes.AplicarAcoesExclusivasOperacao();

                if (usuario.Roles.Contains(ERoles.GERENCIA))
                    opcoes.AplicarAcoesExclusivasGerencia();

                if (usuario.Roles.Contains(ERoles.CONTROLE_DE_RISCO))
                    opcoes.AplicarAcoesExclusivasControleDeRisco();

                if (usuario.Roles.Contains(ERoles.GERENCIA) ||
                    usuario.Roles.Contains(ERoles.CONTROLE_DE_RISCO))
                    opcoes.AplicarAcoesReprovarECorrecao();
            }

            opcoes.Add(new Opcao(Acoes.SAIR, "Sair"));
            return opcoes;
        }


        private static void Rotinas(User usuario, List<Opcao> opcoes, ClientService clienteService, FluxoDeAprovacaoService fluxoService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"--- Seja bem vindo(a) {usuario.Name}! ---");
            Console.ResetColor();

            Console.Write("Perfils do usuário: ");
            var perfils = usuario
                                .Roles
                                .Select(p => p.ToString());

            Console.WriteLine(String.Join(", ", perfils));
            Console.WriteLine("\n");



            if (usuario.Roles.Contains(ERoles.GERENCIA) || usuario.Roles.Contains(ERoles.CONTROLE_DE_RISCO) || usuario.Roles.Contains(ERoles.ADMINISTRACAO))
            {
                IList<Client> clientesParaAprovar = clienteService.GetAllToApproveClients(usuario);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-- Clientes para Aprovar --");
                Console.ResetColor();

                foreach (var cliente in clientesParaAprovar)
                {
                    Console.WriteLine($"Id: {cliente.Id,5}\t\tNome: {cliente.Name,20}\t\tStatus: {cliente.CurrentStatus,10}\t\tInternacional: {cliente.IsInternacional,5}");
                }
            }


            if (usuario.Roles.Contains(ERoles.OPERACAO) || usuario.Roles.Contains(ERoles.ADMINISTRACAO))
            {
                IList<Client> clientesParaOperacao = clienteService.GetAllOperationClients(usuario);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-- Clientes para Operação --");
                Console.ResetColor();

                foreach (var cliente in clientesParaOperacao)
                {
                    Console.WriteLine($"Id: {cliente.Id,5}\t\tNome: {cliente.Name,20}\t\tStatus: {cliente.CurrentStatus,10}\t\tInternacional: {cliente.IsInternacional,5}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Clientes Finalizados --");
            Console.ResetColor();

            IList<Client> clientesFinalizados = clienteService.GetAllFinalizedClients();

            foreach (var cliente in clientesFinalizados)
            {
                Console.WriteLine($"Id: {cliente.Id,5}\t\tNome: {cliente.Name,20}\t\tStatus: {cliente.CurrentStatus,10}\t\tInternacional: {cliente.IsInternacional,5}");
            }


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n-- Opções disponiveis --");

            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < opcoes.Count; i++)
            {
                Opcao opcao = opcoes[i];

                Console.WriteLine($"{i + 1} - {opcao.Descricao}");

            }
            Console.ResetColor();

            Console.WriteLine("\nEscolha uma opção:");
            int.TryParse(Console.ReadLine(), out int indexEscolhido);
            if (indexEscolhido <= 0 || indexEscolhido - 1 > opcoes.Count - 1)
                throw new ArgumentException("Opção inválida!");

            Opcao opcaoEscolhida = opcoes[indexEscolhido - 1];



            if (opcaoEscolhida.Acao == Acoes.SAIR)
            {
                Environment.Exit(0);
            }
            else if (opcaoEscolhida.Acao == Acoes.CADASTRAR_NOVO_CLIENTE)
            {
                CadastrarCliente(usuario, clienteService);
            }
            else
            {
                EscolherAcao(opcaoEscolhida, usuario, clienteService, fluxoService);
            }
        }

        public static void ListarCliente(Client cliente)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"-- Consulta do Cliente {cliente.Id} --");
            Console.ResetColor();

            Console.WriteLine($"Nome: {cliente.Name}");
            Console.WriteLine($"Status Atual: {cliente.CurrentStatus}");
            Console.WriteLine($"Internacional: {cliente.IsInternacional}");
            Console.WriteLine($"CPF: {cliente.CPF}");
            Console.WriteLine($"Data de Nascimento: {cliente.BirthDate.ToShortDateString()}");
            Console.WriteLine($"Email: {cliente.Email}");
            Console.WriteLine($"Cidade: {cliente.Adress.City.Description}");
            Console.WriteLine($"Rua: {cliente.Adress.Street}");
            Console.WriteLine($"Bairro: {cliente.Adress.District}");
            Console.WriteLine($"Numero: {cliente.Adress.Number}");
            Console.WriteLine($"Complemento: {cliente.Adress.Complement}");
            Console.WriteLine($"Cep: {cliente.Adress.Cep}");
            Console.WriteLine($"RG: {cliente.RG}");

            Console.WriteLine("\nPressione Enter para continuar...");
            Console.ReadLine();
        }

        private static void CadastrarCliente(User usuario, ClientService clienteService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Cadastro de Cliente --");
            Console.ResetColor(); ;

            Console.WriteLine("\nNome:");
            string nome = Console.ReadLine();

            Console.WriteLine("CPF: ");
            string cpf = Console.ReadLine();

            Console.WriteLine("Data de Nascimento: ");
            DateTime dataNasc = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Email: ");
            string email = Console.ReadLine();

            Console.WriteLine("Id da Cidade: ");
            int idCidade = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Rua: ");
            string rua = Console.ReadLine();

            Console.WriteLine("Bairro: ");
            string bairro = Console.ReadLine();

            Console.WriteLine("Numero: ");
            int numero = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Complemento: ");
            string complemento = Console.ReadLine();

            Console.WriteLine("Cep: ");
            string cep = Console.ReadLine();

            Console.WriteLine("RG: ");
            string rg = Console.ReadLine();

            var cidade = new City(idCidade);
            var endereco = new Adress(cidade, rua, bairro, numero, complemento, cep);
            var cliente = new Client(nome, cpf, rg, dataNasc, email, endereco);

            clienteService.Register(cliente, usuario);

            Console.Clear();
        }

        private static void AtualizarCliente(Client cliente, ClientService clienteService)
        {
            if (cliente.CurrentStatus.Description != EStatus.EM_CADASTRO &&
                cliente.CurrentStatus.Description != EStatus.CORRECAO_PERFIL)
            {
                throw new InvalidClientException("Cliente inválido para essa operação!");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Atualização de Cliente --");
            Console.ResetColor();
            Console.WriteLine(" -> Caso não deseje mudar o campo, apenas aperte Enter <-\n");


            Console.WriteLine("Nome:");
            Console.WriteLine($"Atual= {cliente.Name}");
            string nome = Console.ReadLine();

            Console.WriteLine("CPF: ");
            Console.WriteLine($"Atual= {cliente.CPF}");
            string cpf = Console.ReadLine();


            Console.WriteLine("Data de Nascimento: ");
            Console.WriteLine($"Atual= {cliente.BirthDate.ToShortDateString()}");
            DateTime.TryParse(Console.ReadLine(), out DateTime dataNasc);

            Console.WriteLine("Email: ");
            Console.WriteLine($"Atual= {cliente.Email}");
            string email = Console.ReadLine();

            Console.WriteLine("Id da Cidade: ");
            Console.WriteLine($"Cidade Atual= {cliente.Adress.City.Description}");
            int idCidade = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Rua: ");
            Console.WriteLine($"Atual= {cliente.Adress.Street}");
            string rua = Console.ReadLine();

            Console.WriteLine("Bairro: ");
            Console.WriteLine($"Atual= {cliente.Adress.District}");
            string bairro = Console.ReadLine();

            Console.WriteLine("Numero: ");
            Console.WriteLine($"Atual= {cliente.Adress.Number}");
            int.TryParse(Console.ReadLine(), out int numero);

            Console.WriteLine("Complemento: ");
            Console.WriteLine($"Atual= {cliente.Adress.Complement}");
            string complemento = Console.ReadLine();

            Console.WriteLine("Cep: ");
            Console.WriteLine($"Atual= {cliente.Adress.Cep}");
            string cep = Console.ReadLine();

            Console.WriteLine("RG: ");
            Console.WriteLine($"Atual= {cliente.RG}");
            string rg = Console.ReadLine();

            var cidade = new City(idCidade);
            var endereco = new Adress(cidade, rua, bairro, numero, complemento, cep);
            var clienteParaAtualizar = new Client(cliente.Id, nome, cpf, rg, dataNasc, email, cliente.CurrentStatus, endereco);

            clienteService.Edit(clienteParaAtualizar);

            Console.Clear();
        }

        private static void EscolherAcao(Opcao opcaoEscolhida, User usuario, ClientService clienteService, FluxoDeAprovacaoService fluxoService)
        {
            Client cliente;
            do
            {
                Console.WriteLine("\nDigite o Id do cliente:");
                var idDigitado = Convert.ToInt32(Console.ReadLine());

                cliente = clienteService.Get(idDigitado);
                if (cliente == null)
                    throw new ArgumentException("Id inválido!");

            } while (cliente == null);


            switch (opcaoEscolhida.Acao)
            {
                case Acoes.ATUALIZAR_CLIENTE:
                    AtualizarCliente(cliente, clienteService);
                    break;
                case Acoes.MANDAR_PARA_GERENCIA:
                case Acoes.APROVAR_CLIENTE_NACIONAL_E_INTERNACIONAL:
                case Acoes.APROVAR_OU_ENVIAR_PARA_RISCO:
                case Acoes.APROVAR_CLIENTE_INTERNACIONAL:
                    fluxoService.AprovarCliente(cliente, usuario);
                    break;
                case Acoes.REPROVAR_CLIENTE:
                    fluxoService.ReprovarCliente(cliente, usuario);
                    break;
                case Acoes.MANDAR_PARA_CORRECAO_DE_PERFIL:
                    fluxoService.EnviarParaCorrecao(cliente, usuario);
                    break;
                case Acoes.CONSULTAR_CLIENTE:
                    ListarCliente(cliente);
                    break;
                default:
                    throw new ArgumentException("Opção inválida!");
            }
            Console.Clear();
        }
    }
}
