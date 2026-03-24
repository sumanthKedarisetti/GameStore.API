
using GameStore.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Interfaces.IServices;

public interface IGameService
{
    Task<IEnumerable<Game>> GetGames();
    Task<Game> GetGamebyId(int id);   
    Task<Game> CreateGame(Game game);
    Task<ActionResult> UpdateGame(int id, Game updatedGame);    
    Task<ActionResult> DeleteGame(int id);
}
   