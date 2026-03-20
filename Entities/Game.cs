namespace GameStore.API.Entities;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }  
    public DateTime ReleaseDate { get; set; }
    public string Developer { get; set; } = string.Empty;   
    public string Publisher { get; set; } = string.Empty;
}