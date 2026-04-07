using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

//* Act as a dbcontext, save query and instances both to the database.
//* Sirve para la comunicacion entre la api y la base de datos, va a darle contexto a cada uno 
//* cuando se quieran comunicar.
public class GameStoreContext(DbContextOptions<GameStoreContext> options)
 : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    
}
