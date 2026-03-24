
using GameStore.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStore.API.Interfaces.IServices;

[ApiController]
[Route("api/allgames")]
public class GameController : ControllerBase
{
     

     public GameController(IGameService gameService)
     {
         _gameService = gameService;
     }
     private readonly IGameService _gameService;

     [HttpGet]
     public async Task<ActionResult<IEnumerable<Game>>> GetGames()
     {
         var result = await _gameService.GetGames();
         return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGamebyId(int id)
    {
        var result = await _gameService.GetGamebyId(id);
        if(result==null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        var result = await _gameService.CreateGame(game);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(int id, Game updatedGame)
    {
       var result= await _gameService.UpdateGame(id, updatedGame);
        if(result is NotFoundResult)
        {
            return NotFound();
        }
        if(result is BadRequestResult)
        {
            return BadRequest();
        }
      return NoContent();
    }

    

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        var result = await _gameService.DeleteGame(id);
        if(result is NotFoundResult)
        {
            return NotFound();
        }
        return NoContent();
    }

}