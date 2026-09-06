using NetwiseTask.Models;

namespace NetwiseTask.Services;

public interface ICatFactService
{
    Task<CatFact> GetCatFactAsync();
    Task<DailyFact> GetDailyFactAsync();
    Task<CatFact> GetNonBannedCatFactAsync();
    Task<CatImage> GetCatImageAsync();

    Task SaveFactAsync(CatFact catFact);

    Task<List<CatFact>> GetHistoryAsync();

    Task<(bool IsFavourite, bool IsBanned)> GetFactStatusAsync(CatFact catFact);

    Task AddFavoriteAsync(CatFact catFact);

    Task RemoveFavoriteAsync(CatFact catFact);

    Task<List<CatFact>> GetFavoritesAsync();

    Task AddBannedAsync(CatFact catFact);

    Task RemoveBannedAsync(CatFact catFact);

    Task<List<CatFact>> GetBannedAsync();
}