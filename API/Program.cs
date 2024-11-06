using API.Extensions;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurarLosCORS(); // Este emtodo es mi extension para configurar los CORS

//Esto se hizo en una extencion del servico arriba
// builder.Services.AddCors(opt => {
//     opt.AddPolicy("CorsPolicy", policy => {
//         policy.AllowAnyHeader().
//         AllowAnyMethod().
//         WithOrigins("https://localhost:4200");
//     });
// });

builder.Services.AddControllers();// Este servicio es para poder usar controladores

// inyección de dependencias para el contexto de la base de datos 
builder.Services.AddDbContext<TiendaContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Codigo para hacer migracion automatica de los modelos a la base de datos con EF Core cada vez que se corra el programa
// Create a new scope
using (var scope = app.Services.CreateScope()) {

    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try {
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
        await TiendaContextSeed.SeedAsync(context, loggerFactory);
    }
    catch (Exception ex) {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy"); // Este metodo es para poder usar los CORS
app.UseHttpsRedirection();// 
app.MapControllers(); // Este metodo es para poder usar controladores en la API
app.Run();


