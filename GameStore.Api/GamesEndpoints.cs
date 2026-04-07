
using GameStore.Api.Dtos;

namespace GameStore.Api;

//* VA CONTENER TODOS LOS ENDPOINTS DE LA API, PARA MANEJAR UN CODIGO MAS LIMPIO.
public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
    new (
        1,
        "Street Fighter II", 
        "Fighting", 
        19.99M, 
        new DateOnly(1992, 7, 15)),
    new (
        2,
        "Final Fantasy VII Rebirth",
        "RPG",
        69.99M,
        new DateOnly(2024, 2, 29)),
    new (
        3,
        "Doom Eternal",
        "FPS",
        49.99M,
        new DateOnly(2020, 3, 20))
    ];



    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //* GET /games - (endpoint)
        group.MapGet("/", () => games);

        //* GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        //* POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {

            

            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
        });

        //* PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            //* SE LE DA LA POSIBILIDAD DE FALLAR EN LA REQUEST CON UN CONDICIONAL.
            if (index == -1) return Results.NotFound();

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //* DELETE /games/id
        //* NO NECESITA DTOS PARA ELIMINAR UN JUEGO DEL ARRAY.
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent(); 
        });
    }
}
