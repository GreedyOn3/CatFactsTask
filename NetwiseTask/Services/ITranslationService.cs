namespace NetwiseTask.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateToPolishAsync(string text);
    }
}
