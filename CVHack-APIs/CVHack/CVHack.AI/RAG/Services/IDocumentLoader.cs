namespace CVHack.AI
{
    public interface IDocumentLoader
    {
        Task<IReadOnlyList<Document>> LoadAsync();
    }
}
