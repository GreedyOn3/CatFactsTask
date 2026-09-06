using Microsoft.AspNetCore.Mvc.RazorPages;
using NetwiseTask.Models;
using NetwiseTask.Services;

namespace NetwiseTask.Pages;

public class ListModel : PageModel
{
    private readonly ICatFactService _catFactService;

    public ListModel(ICatFactService catFactService)
    {
        _catFactService = catFactService;
    }

    public List<CatFact> Facts { get; set; } = [];

    public string ListType { get; set; } = "history";

    public string ListTitle { get; set; } = "Historia";

    public string Search { get; set; } = "";

    public async Task OnGetAsync(
        string? type,
        string? search)
    {
        ListType = type?.ToLower() switch
        {
            "favorites" => "favorites",
            "banned" => "banned",
            _ => "history"
        };

        Search = search?.Trim() ?? string.Empty;

        switch (ListType)
        {
            case "favorites":
                Facts = await _catFactService.GetFavoritesAsync();
                ListTitle = "Moje ulubione fakty";
                break;

            case "banned":
                Facts = await _catFactService.GetBannedAsync();
                ListTitle = "Znienawidzone fakty";
                break;

            default:
                Facts = await _catFactService.GetHistoryAsync();
                ListTitle = "Historia faktów";
                break;
        }

        Facts.Reverse();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            Facts = Facts
                .Where(x => x.Fact.Contains(
                    Search,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}