using System.Reflection;
using System.Text;
using API.Extensions;
using API.Helpers;
using AspNetCoreRateLimit;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurarLosCORS(); // Mi extension para configurar los CORS
builder.Services.InjeccionDeDependencias(); // Mi extension para agregar los servicios
builder.Services.ConfigurarLimitesDePeticiones();//Confifurar con libreria AspNetCoreRateLimit
builder.Services.AddControllers(); // Este servicio es para poder usar controladores tradicionales

//con este codigo se habilita el formato XML en la API y que acepte el header Accept: application/xml
// builder.Services.AddControllers(options => {
//     options.RespectBrowserAcceptHeader = true;
//     options.ReturnHttpNotAcceptable = true;
// }).AddXmlSerializerFormatters();

builder.Services.AddSwaggerGen(); // Este metodo es para poder usar la documentacion de swagger
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // este metodo busca en la clase donde se esta ejecutando para encontrar los perfiles de automapper. Curso Udemy
builder.Services.ConfigurarVersionamientoDeAPI(); // Este metodo es mi extension para configurar las versiones de la API
builder.Services.AddEndpointsApiExplorer();  //Este metodo es para poder usar la documentacion de swagger
builder.Services.AddAuthenticationYConfigurarJWT(builder.Configuration, builder.Environment); //Este metodo agrega el Services.AddAuthentication() y configura el JWT


builder.Services.AddDbContext<TiendaContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {

    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try {
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
        //Este metodo es para agregar datos a la base de datos 
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

app.UseIpRateLimiting(); // Este metodo es para poder usar el RateLimiter de la libreria de AspNetCoreRateLimit
app.UseCors("CorsPolicy"); // Este metodo es para poder usar los CORS
app.UseAuthentication(); // Este metodo es para poder usar la autenticacion por ejemplo JWT, Bearer, 0Auth, AzureAD,
app.UseAuthorization(); // Este metodo es para poder usar la autorizacion por ejemplo Roles, Claims, Politicas, que se leen desde el token JWT o de la base de datos
app.UseHttpsRedirection();// esto es para redirigir a https ejemplo: http://localhost:5000 a https://localhost:5001
app.MapControllers(); // Este metodo es para que reemplaze la palabra controller por el nombre del controlador ejemplo: UsuariosController -> Usuarios
app.Run();
