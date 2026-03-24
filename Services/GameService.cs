
using GameStore.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GameStore.API.Interfaces.IServices;

namespace GameStore.API.Services;

public class GameService : IGameService
{
    private readonly GameStoreContext _context;
    public GameService(GameStoreContext context)
    {
      _context = context;
    }
    public async Task<IEnumerable<Game>> GetGames()
    {
        var games=await _context.Games.AsNoTracking().ToListAsync();
        return games;
    }

    public async Task<Game> GetGamebyId(int id)
    {
        var game=await _context.Games.AsNoTracking().FirstOrDefaultAsync(game=>game.Id==id);
        if(game==null)
        {
            return null;
        }
        return game;
    }
    public async Task<Game> CreateGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }
    public async Task<ActionResult> UpdateGame(int id, Game updatedGame)
    {
        if(id!=updatedGame.Id)
        {
            return new BadRequestResult();
        }
        _context.Entry(updatedGame).State=EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if(!GameExists(id))
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }
        return new NoContentResult();
    }
    private bool GameExists(int id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
    public async Task<ActionResult> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return new NotFoundResult();
        }
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }
}
