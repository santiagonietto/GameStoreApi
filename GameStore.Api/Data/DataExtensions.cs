

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

        //* DbContext tiene un Scoped service de por vida porque:
        //* 1. Se asegura de que una nueva instancia de DbContext is creada por request
        //* 2. Las conexiones con la DB son limitadas y un recurso caro.
        //* 3. DbContext no es seguro para subprocesos. Scoped evita errores que pasan todo el tiempo.
        //* 4. Hace mas facil manejar transacciones y garantizar la coherencia de los datos.
        //* 5. Reusar una instancia de DbContext puede llevar a incrementar el uso de memoria.
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
