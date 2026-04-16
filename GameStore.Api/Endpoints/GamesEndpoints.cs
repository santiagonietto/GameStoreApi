
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api;

//* VA CONTENER TODOS LOS ENDPOINTS DE LA API, PARA MANEJAR UN CODIGO MAS LIMPIO.
public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";


    //* Base de datos antes de las inyecciones de dependencias. Hardcodeada.
    // private static readonly List<GameSummaryDto> games = [
    // new (
    //     1,
    //     "Street Fighter II", 
    //     "Fighting", 
    //     19.99M, 
    //     new DateOnly(1992, 7, 15)),
    // new (
    //     2,
    //     "Final Fantasy VII Rebirth",
    //     "RPG",
    //     69.99M,
    //     new DateOnly(2024, 2, 29)),
    // new (
    //     3,
    //     "Doom Eternal",
    //     "FPS",
    //     49.99M,
    //     new DateOnly(2020, 3, 20))
    // ];



    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //* GET /games - (endpoint)
        group.MapGet("/", async (GameStoreContext dbContext) 
        => await dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name ?? "Unknown",
                                game.Price,
                                game.ReleaseDate
                            ))
                            .AsNoTracking()
                            .ToListAsync());

        //* GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game =  await dbContext.Games.FindAsync(id);
            
            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
        .WithName(GetGameEndpointName);

        //* POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            //* Le aviso a EFCore que tiene que trackear un nuevo juego que se va insertar 
            //* en la base de datos.
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(); //* Transforma los datos mandados a idioma sql para que sqlserver los entienda. 
            //* Async habre la puerta a que pueda recibir Tasks<int> de manera asincronica.

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = gameDto.Id}, gameDto);
        });

        //* PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            //* SE LE DA LA POSIBILIDAD DE FALLAR EN LA REQUEST CON UN CONDICIONAL.
            if (existingGame is null) return Results.NotFound();

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //* DELETE /games/id
        //* NO NECESITA DTOS PARA ELIMINAR UN JUEGO DEL ARRAY.
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //* Borra todos los juegos que coincidan con el contexto exacto que se le provee.
            await dbContext.Games   
                            .Where(game => game.Id == id)
                            .ExecuteDeleteAsync(); //* Despues de esta linea el contexto apuntado se borra.

            return Results.NoContent(); 
        });
    }
}
