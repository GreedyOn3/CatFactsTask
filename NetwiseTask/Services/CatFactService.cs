using System.Net.Http.Json;
using NetwiseTask.Models;

namespace NetwiseTask.Services;

public class CatFactService : ICatFactService
{
    private readonly HttpClient _httpClient;
    private readonly string _dataDirectory;

    public CatFactService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _dataDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "Data");

        Directory.CreateDirectory(_dataDirectory);
    }

    public async Task<CatFact> GetCatFactAsync()
    {
        var catFact = await _httpClient.GetFromJsonAsync<CatFact>(
            "https://catfact.ninja/fact");

        if (catFact is null)
        {
            throw new InvalidOperationException(
                "Nie udało się pobrać faktu o kocie.");
        }

        return catFact;
    }

    public async Task<DailyFact> GetDailyFactAsync()
    {
        var filePath = Path.Combine(
            _dataDirectory,
            "dailyfact.txt");

        if (File.Exists(filePath))
        {
            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length > 0)
            {
                var lastLine = lines.LastOrDefault();

                if (!string.IsNullOrWhiteSpace(lastLine))
                {
                    var dailyFact = ParseDailyFact(lastLine);

                    if (dailyFact is not null &&
                        dailyFact.Date.Date == DateTime.Today)
                    {
                        return dailyFact;
                    }
                }
            }
        }

        var catFact = await GetNonBannedCatFactAsync();
        var catImage = await GetCatImageAsync();

        var factTitles = new[]
        {
        "Czy wiedziałeś, że...",
        "Nie spadnij z krzesełka ale...",
        "Niesamowite..."
    };

        var title = factTitles[
            Random.Shared.Next(factTitles.Length)
        ];

        var newDailyFact = new DailyFact
        {
            Date = DateTime.Today,
            Fact = catFact.Fact,
            Length = catFact.Length,
            ImageUrl = catImage.Url,
            Title = title
        };

        var line = string.Join(
            "|",
            newDailyFact.Date.ToString("yyyy-MM-dd"),
            newDailyFact.Fact,
            newDailyFact.Length,
            newDailyFact.ImageUrl,
            newDailyFact.Title);

        await File.AppendAllTextAsync(
            filePath,
            line + Environment.NewLine);

        return newDailyFact;
    }

    public async Task<CatFact> GetNonBannedCatFactAsync()
    {
        while (true)
        {
            var catFact = await GetCatFactAsync();
            var status = await GetFactStatusAsync(catFact);

            var isBanned = status.IsBanned;

            if (isBanned)
                continue;

            await SaveFactAsync(catFact);
            return catFact;
        }
    }

    public async Task<CatImage> GetCatImageAsync()
    {
        var catImage = await _httpClient.GetFromJsonAsync<CatImage>(
            "https://cataas.com/cat?json=true");

        if (catImage is null ||
            string.IsNullOrWhiteSpace(catImage.Url))
        {
            throw new InvalidOperationException(
                "Nie udało się pobrać zdjęcia kota.");
        }

        return catImage;
    }

    public async Task SaveFactAsync(CatFact catFact)
    {
        await AppendToFileAsync(
            "catfacts.txt",
            catFact);
    }

    public async Task<List<CatFact>> GetHistoryAsync()
    {
        return await ReadFromFileAsync("catfacts.txt");
    }

    public async Task<(bool IsFavourite, bool IsBanned)> GetFactStatusAsync(CatFact catFact)
    {
        var favorites = await GetFavoritesAsync();
        var banned = await GetBannedAsync();

        var isFavorite = favorites.Any(x =>
            x.Fact.Equals(
                catFact.Fact,
                StringComparison.Ordinal));

        var isBanned = banned.Any(x =>
            x.Fact.Equals(
                catFact.Fact,
                StringComparison.Ordinal));

        return (isFavorite, isBanned);
    }

    public async Task AddFavoriteAsync(CatFact catFact)
    {
        await RemoveBannedAsync(catFact);
        await AppendToFileAsync("favorites.txt", catFact);
    }

    public async Task RemoveFavoriteAsync(CatFact catFact)
    {
        await RemoveFromFileAsync(
            "favorites.txt",
            catFact);
    }

    public async Task<List<CatFact>> GetFavoritesAsync()
    {
        return await ReadFromFileAsync("favorites.txt");
    }

    public async Task AddBannedAsync(CatFact catFact)
    {
        await RemoveFavoriteAsync(catFact);
        await AppendToFileAsync("banned.txt", catFact);
    }

    public async Task RemoveBannedAsync(CatFact catFact)
    {
        await RemoveFromFileAsync(
            "banned.txt",
            catFact);
    }

    public async Task<List<CatFact>> GetBannedAsync()
    {
        return await ReadFromFileAsync("banned.txt");
    }

    private async Task AppendToFileAsync(
        string fileName,
        CatFact catFact)
    {
        var filePath = Path.Combine(
            _dataDirectory,
            fileName);

        var line = $"{catFact.Fact}|{catFact.Length}";

        await File.AppendAllTextAsync(
            filePath,
            line + Environment.NewLine);
    }

    private async Task<List<CatFact>> ReadFromFileAsync(
        string fileName)
    {
        var filePath = Path.Combine(
            _dataDirectory,
            fileName);

        if (!File.Exists(filePath))
        {
            return [];
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        var facts = new List<CatFact>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var fact = ParseCatFact(line);

            if (fact is not null)
            {
                facts.Add(fact);
            }
        }

        return facts;
    }

    private async Task RemoveFromFileAsync(
        string fileName,
        CatFact catFact)
    {
        var facts = await ReadFromFileAsync(fileName);

        facts.RemoveAll(x =>
            x.Fact.Equals(
                catFact.Fact,
                StringComparison.Ordinal));

        var filePath = Path.Combine(
            _dataDirectory,
            fileName);

        var lines = facts.Select(
            x => $"{x.Fact}|{x.Length}");

        await File.WriteAllLinesAsync(
            filePath,
            lines);
    }

    private static CatFact? ParseCatFact(string line)
    {
        var separatorIndex = line.LastIndexOf('|');

        if (separatorIndex == -1)
        {
            return null;
        }

        var fact = line[..separatorIndex];

        var lengthText = line[(separatorIndex + 1)..];

        if (!int.TryParse(lengthText, out var length))
        {
            return null;
        }

        return new CatFact
        {
            Fact = fact,
            Length = length
        };
    }

    private static DailyFact? ParseDailyFact(string line)
    {
        var parts = line.Split('|');

        if (parts.Length < 5)
        {
            return null;
        }

        if (!DateTime.TryParse(
                parts[0],
                out var date))
        {
            return null;
        }

        if (!int.TryParse(
                parts[^3],
                out var length))
        {
            return null;
        }

        return new DailyFact
        {
            Date = date,
            Fact = parts[1],
            Length = length,
            ImageUrl = parts[^2],
            Title = parts[^1]
        };
    }
}