namespace GameStore.API.Entities;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }  
    public DateTime ReleaseDate { get; set; }
    public string Developer { get; set; }   
    public string Publisher { get; set; }
}