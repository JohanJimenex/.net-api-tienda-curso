// using System.Reflection;
// using System.Threading.RateLimiting;
// using API.Extensions;
// using AspNetCoreRateLimit;
// using Infrastructure.data;
// using Microsoft.AspNetCore.RateLimiting;
// using Microsoft.EntityFrameworkCore;


// var builder = WebApplication.CreateBuilder(args);

// builder.Services.ConfigurarLosCORS(); // Este metodo es mi extension para configurar los CORS
// builder.Services.InjeccionDeDependencias(); // Este metodo es mi extension para agregar los servicios

// //Esto se hizo en una extencion del servico arriba
// // builder.Services.AddCors(opt => {
// //     opt.AddPolicy("CorsPolicy", policy => {
// //         policy.AllowAnyHeader().
// //         AllowAnyMethod().
// //         WithOrigins("https://localhost:4200");
// //     });
// // });
// builder.Services.ConfigurarVersionesdeAPI(); // Este metodo es mi extension para configurar las versiones de la API

// builder.Services.AddControllers()// Este servicio es para poder usar controladores
// .AddXmlSerializerFormatters(); // Este metodo es para poder usar el formato XML en la API

// // inyección de dependencias para el contexto de la base de datos 
// builder.Services.AddDbContext<TiendaContext>(options => {
//     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
// });


// // builder.Services.AddEndpointsApiExplorer();  //Este metodo fue configurado en la extension de ApplicationServicesExtensions 
// builder.Services.AddSwaggerGen();

// // Este metodo es para poder usar automapper
// // builder.Services.AddAutoMapper(typeof(Program)); // este metodo busca en la clase Program para encontrar los perfiles de automapper felipe Gavilan
// builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // este metodo busca en la clase donde se esta ejecutando para encontrar los perfiles de automapper. Curso Udemy

// //Codigo antivo de .net core, con este metodo se agrega el RateLimiter para limitar las peticiones a la API 
// //recurda agregar la anotacion [EnableRateLimiting("fixed")] en el controlador que se quiera limitar
// // builder.Services.AddRateLimiter(_ => _
// //     .AddFixedWindowLimiter(policyName: "fixed", options => {
// //         options.PermitLimit = 1;
// //         options.Window = TimeSpan.FromSeconds(12);
// //         options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
// //         options.QueueLimit = 0;
// //     }));

// //Confifurar con libreria AspNetCoreRateLimit
// builder.Services.ConfigurarLimitesDePeticiones();


// var app = builder.Build();

// // Codigo para hacer migracion automatica de los modelos a la base de datos con EF Core cada vez que se corra el programa
// // el Using es para que se libere la memoria una vez que se termine de ejecutar el codigo ejecuta el metodo Dispose()
// using (var scope = app.Services.CreateScope()) {

//     var services = scope.ServiceProvider;
//     var loggerFactory = services.GetRequiredService<ILoggerFactory>();

//     try {
//         var context = services.GetRequiredService<TiendaContext>();
//         await context.Database.MigrateAsync();
//         await TiendaContextSeed.SeedAsync(context, loggerFactory);
//     }
//     catch (Exception ex) {
//         var logger = loggerFactory.CreateLogger<Program>();
//         logger.LogError(ex, "An error occurred creating the DB.");
//     }
// }

// if (app.Environment.IsDevelopment()) {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // Este metodo es para poder usar el RateLimiter nativo de .net core
// // app.UseRateLimiter();
// // app.UseApiVersioning(); // Este metodo es para poder usar versionado en la API
// app.UseIpRateLimiting(); // Este metodo es para poder usar el RateLimiter de la libreria de AspNetCoreRateLimit
// app.UseCors("CorsPolicy"); // Este metodo es para poder usar los CORS
// app.UseHttpsRedirection();// 
// app.MapControllers(); // Este metodo es para poder usar controladores en la API
// app.Run();


