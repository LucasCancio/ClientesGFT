namespace ClientesGFT.ConsoleApplication
{
    public class Opcao
    {
        public Opcao(Acoes acao, string descricao)
        {
            Acao = acao;
            Descricao = descricao;
        }

        public Acoes Acao { get; set; }
        public string Descricao { get; set; }
    }
}
