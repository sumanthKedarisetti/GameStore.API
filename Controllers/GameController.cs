
using GameStore.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/allgames")]
public class GameController : ControllerBase
{
     private readonly GameStoreContext _context;

     public GameController(GameStoreContext context)
     {
         _context = context;
     }

     [HttpGet]
     public async Task<ActionResult<IEnumerable<Game>>> GetGames()
     {
         var games = await _context.Games.AsNoTracking().ToListAsync();
         return Ok(games);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }
    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        _context.Games.Add(game);//Add the new game to the context
        await _context.SaveChangesAsync();//save the changes to the database
        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);//return a 201 created response with the location of the new game
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(int id, Game updatedGame)
    {
        if (id != updatedGame.Id)
        {
            return BadRequest();
        }

        _context.Entry(updatedGame).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GameExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool GameExists(int id)
    {
        return _context.Games.Any(e => e.Id == id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}