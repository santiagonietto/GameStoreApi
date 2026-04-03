namespace GameStore.Api.Dtos;

//* Un DTO es un contrato entre el cliente y el servidor que representa
//* un acuerdo compartido sobre como los datos van a ser enviados y usados.

//* Una clase de tipo "record" es una clase optimizada para almacenar datos inmutables
//* y modelar estructuras de datos simples. Ideales para DTOs(Data Transfer Objects).

public record GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
