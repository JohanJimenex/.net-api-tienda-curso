using Infrastructure.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Este servicio es para poder usar controladores
builder.Services.AddControllers();

// Este archivo se pueden inyectar clases globales como en angular con app.module.ts
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
    }
    catch (Exception ex) {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection(); 
app.MapControllers(); // Este metodo es para poder usar controladores en la API
app.Run();


