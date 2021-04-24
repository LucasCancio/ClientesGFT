namespace ClientesGFT.Domain.Util
{
    public static class DocumentFixer
    {
        public static string Fix(string document)
        {
            return document?.Replace(".", "").Replace("-", "");
        }
    }
}
