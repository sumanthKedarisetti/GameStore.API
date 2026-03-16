using Microsoft.EntityFrameworkCore;
using GameStore.API.Entities;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
}