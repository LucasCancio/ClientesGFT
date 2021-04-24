using System.Collections.Generic;

namespace ClientesGFT.ConsoleApplication
{
    public static class AplicarAcoes
    {
        public static List<Opcao> AplicarAcoesExclusivasOperacao(this List<Opcao> opcoes)
        {
            opcoes.Add(new Opcao(Acoes.CADASTRAR_NOVO_CLIENTE, "Cadastrar novo cliente"));
            opcoes.Add(new Opcao(Acoes.ATUALIZAR_CLIENTE, "Atualizar cliente"));
            opcoes.Add(new Opcao(Acoes.MANDAR_PARA_GERENCIA, "Mandar para Gerência"));

            return opcoes;
        }

        public static List<Opcao> AplicarAcoesExclusivasGerencia(this List<Opcao> opcoes)
        {
            opcoes.Add(new Opcao(Acoes.APROVAR_OU_ENVIAR_PARA_RISCO, "Aprovar cliente"));
            return opcoes;
        }

        public static List<Opcao> AplicarAcoesExclusivasControleDeRisco(this List<Opcao> opcoes)
        {
            opcoes.Add(new Opcao(Acoes.APROVAR_CLIENTE_INTERNACIONAL, "Aprovar cliente"));
            return opcoes;
        }

        public static List<Opcao> AplicarAcoesReprovarECorrecao(this List<Opcao> opcoes)
        {
            opcoes.Add(new Opcao(Acoes.REPROVAR_CLIENTE, "Reprovar cliente"));
            opcoes.Add(new Opcao(Acoes.MANDAR_PARA_CORRECAO_DE_PERFIL, "Mandar para Correção de Perfil"));
            return opcoes;
        }


        public static List<Opcao> AplicarAcoesExclusivasAdministracao(this List<Opcao> opcoes)
        {
            opcoes.Add(new Opcao(Acoes.APROVAR_CLIENTE_NACIONAL_E_INTERNACIONAL, "Aprovar cliente"));
            return opcoes;
        }
    }
}
