namespace GameStore.API.DTOs;

public record CreateGameDTO
(string Name, 
string Genre, 
decimal Price,
string Developer, 
string Publisher );