using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetwiseTask.Models;
using NetwiseTask.Services;

namespace NetwiseTask.Pages;

public class FactModel : PageModel
{
    private readonly ICatFactService _catFactService;
    private readonly ITranslationService _translationService;

    public FactModel(
        ICatFactService catFactService,
        ITranslationService translationService)
    {
        _catFactService = catFactService;
        _translationService = translationService;
    }

    public CatFact? CatFact { get; set; }

    public CatImage? CatImage { get; set; }

    public string FactTitle { get; set; } = "";

    public string? PolishFact { get; set; }

    public bool IsPolish { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsBanned { get; set; }

    private static readonly string[] FactTitles =
    [
        "Czy wiedziałeś, że...",
        "Nie spadnij z krzesełka ale...",
        "Niesamowite..."
    ];


    public async Task OnGetAsync(string? fact, int? length, string? type)
    {
        if (type == "daily")
        {
            await GetDailyFactAsync();
            return;
        }

        if (!string.IsNullOrWhiteSpace(fact))
        {
            CatFact = new CatFact
            {
                Fact = fact,
                Length = length ?? fact.Length
            };

            CatImage = await _catFactService.GetCatImageAsync();

            FactTitle = FactTitles[
                Random.Shared.Next(FactTitles.Length)
            ];

            await SetFactStatusAsync();

            IsPolish = false;
            PolishFact = null;

            return;
        }

        await GetNewFactAsync();
    }


    public async Task<IActionResult> OnPostNextAsync()
    {
        await GetNewFactAsync();

        return Page();
    }


    public async Task<IActionResult> OnPostFavoriteAsync(
     string fact,
     int length,
     string imageUrl,
     string factTitle)
    {
        CatFact = new CatFact
        {
            Fact = fact,
            Length = length
        };

        CatImage = new CatImage
        {
            Url = imageUrl
        };

        FactTitle = factTitle;

        var status = await _catFactService
            .GetFactStatusAsync(CatFact);

        if (status.IsFavourite)
        {
            await _catFactService.RemoveFavoriteAsync(CatFact);
        }
        else
        {
            await _catFactService.AddFavoriteAsync(CatFact);
        }

        await SetFactStatusAsync();

        return Page();
    }


    public async Task<IActionResult> OnPostBannedAsync(
    string fact,
    int length,
    string imageUrl,
    string factTitle)
    {
        CatFact = new CatFact
        {
            Fact = fact,
            Length = length
        };

        CatImage = new CatImage
        {
            Url = imageUrl
        };

        FactTitle = factTitle;

        var status = await _catFactService
            .GetFactStatusAsync(CatFact);

        if (status.IsBanned)
        {
            await _catFactService.RemoveBannedAsync(CatFact);
        }
        else
        {
            await _catFactService.AddBannedAsync(CatFact);
        }

        await SetFactStatusAsync();

        return Page();
    }


    public async Task<IActionResult> OnPostTranslateAsync(
        string fact,
        int length,
        string imageUrl,
        string factTitle)
    {
        CatFact = new CatFact
        {
            Fact = fact,
            Length = length
        };

        CatImage = new CatImage
        {
            Url = imageUrl
        };

        FactTitle = factTitle;

        PolishFact = await _translationService
            .TranslateToPolishAsync(fact);

        IsPolish = true;

        await SetFactStatusAsync();

        return Page();
    }


    public async Task<IActionResult> OnPostEnglishAsync(
        string fact,
        int length,
        string imageUrl,
        string factTitle)
    {
        CatFact = new CatFact
        {
            Fact = fact,
            Length = length
        };

        CatImage = new CatImage
        {
            Url = imageUrl
        };

        FactTitle = factTitle;

        IsPolish = false;

        await SetFactStatusAsync();

        return Page();
    }


    private async Task GetNewFactAsync()
    {
        CatFact = await _catFactService.GetNonBannedCatFactAsync();

        CatImage = await _catFactService.GetCatImageAsync();

        FactTitle = FactTitles[
            Random.Shared.Next(FactTitles.Length)
        ];

        await SetFactStatusAsync();

        IsPolish = false;
        PolishFact = null;
    }

    private async Task GetDailyFactAsync()
    {
        var dailyFact = await _catFactService.GetDailyFactAsync();

        CatFact = new CatFact
        {
            Fact = dailyFact.Fact,
            Length = dailyFact.Length
        };

        CatImage = new CatImage
        {
            Url = dailyFact.ImageUrl
        };

        FactTitle = dailyFact.Title;

        await SetFactStatusAsync();

        IsPolish = false;
        PolishFact = null;
    }


    private async Task SetFactStatusAsync()
    {
        if (CatFact is null)
        {
            IsFavorite = false;
            IsBanned = false;

            return;
        }

        var status = await _catFactService.GetFactStatusAsync(CatFact);

        IsFavorite = status.IsFavourite;
        IsBanned = status.IsBanned;
    }
}