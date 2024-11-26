using System.Reflection;
using System.Text;
using API.Extensions;
using API.Helpers;
using AspNetCoreRateLimit;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Configura Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()// este metodo es para que los logs se muestren en la consola ya que Serilog por defecto no muestra los logs en la consola
    .WriteTo.File("../logs-klk/log-.txt", rollingInterval: RollingInterval.Day) //Esto tmabien se puede configurar en el appsettings.json
    .CreateLogger();

builder.Logging.AddSerilog(dispose: true);

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
// builder.Services.AddAuthorization(); // Este metodo es para poder usar la autorizacion por ejemplo Roles, Claims, Politicas, que se leen desde el token JWT o de la base de datos

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
// Esto es para poder usar la Anotacion [Authorize] en los controladores, haciendo que busque el token y validandolo,
// tambien [Authorize(Roles = "Administrador")] buscando los roles en el Claims del token, o Politicas, que se leen desde el token JWT o de la base de datos
app.UseAuthorization();
app.UseHttpsRedirection();// esto es para redirigir a https ejemplo: http://localhost:5000 a https://localhost:5001
app.MapControllers(); // Este metodo es para que reemplaze la palabra controller por el nombre del controlador ejemplo: UsuariosController -> Usuarios
app.Run();
