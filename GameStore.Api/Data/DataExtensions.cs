

using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;
//! AUTOMATICAMENTE ACTUALIZA LA BASE DE DATOS CON LOS NUEVOS DATOS CARGADOS O TABLAS. SI NECESIDAD DE CORRER EL UPDATE CADA VEZ QUE SE ACTUALIZA ALGO EN LA BASE DE DATOS.
public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbcontext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre {Name = "Fighting"},
                        new Genre {Name = "RPG"},
                        new Genre {Name = "Platformer"},
                        new Genre {Name = "Racing"},
                        new Genre {Name = "Sports"}
                    );

                    context.SaveChanges(); 
                }
            })
        );
    }
}
