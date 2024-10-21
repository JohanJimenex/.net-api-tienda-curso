// using Infrastructure.data;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// // Este archivo se pueden inyectar clases globales como en angular con app.module.ts
// // inyección de dependencias para el contexto de la base de datos 
// builder.Services.AddDbContext<TiendaContext>(options => {
//     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
// });

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // Codigo para hacer migracion automatica de los modelos a la base de datos con EF Core cada vez que se corra el programa
// // Create a new scope
// using (var scope = app.Services.CreateScope()) {

//     var services = scope.ServiceProvider;
//     var loggerFactory = services.GetRequiredService<ILoggerFactory>();

//     try {
//         var context = services.GetRequiredService<TiendaContext>();
//         await context.Database.MigrateAsync();
//     }
//     catch (Exception ex) {
//         var logger = loggerFactory.CreateLogger<Program>();
//         logger.LogError(ex, "An error occurred creating the DB.");
//     }
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () => {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();


// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

