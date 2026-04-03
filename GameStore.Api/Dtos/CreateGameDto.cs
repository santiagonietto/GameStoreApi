namespace GameStore.Api.Dtos;

//* No se le crea un id al post porque usualmente eso lo crea el servidor

public record CreateGameDto(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
