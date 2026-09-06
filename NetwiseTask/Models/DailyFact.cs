namespace NetwiseTask.Models;

public class DailyFact
{
    public DateTime Date { get; set; }

    public string Fact { get; set; } = "";

    public int Length { get; set; }

    public string ImageUrl { get; set; } = "";

    public string Title { get; set; } = "";
}