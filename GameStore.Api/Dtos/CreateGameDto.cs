using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

//* No se le crea un id al post porque usualmente eso lo crea el servidor

//*[REQUIRED] para exigir que el campo contenga lo esperado.
//*[StringLength(largo)] para limitar el largo del string.
public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Range(1, 50)] int GenreId,
    [Range(1, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);
