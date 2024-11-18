using System;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace API.Extensions;

public static class ApplicationServicesExtensions {

    //pasarle como parametro el IServiceCollection services con this para que sea una extension, y poder usar builder.Services.ConfigurarLosCORS();
    public static void ConfigurarLosCORS(this IServiceCollection services) {
        services.AddCors(opt => {
            opt.AddPolicy("CorsPolicy", policy => {
                policy.AllowAnyHeader().
                AllowAnyMethod().
                WithOrigins("https://localhost:4200");
            });
        });
    }

    public static void InjeccionDeDependencias(this IServiceCollection services) {

        // services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>)); //se usa typeof porque la interfaz y la clase usan tipos genericos <T>
        // services.AddScoped<IProductoRepository, ProductoRepositoryImpl>();
        // services.AddScoped<ICategoriaRepository, CategoriaRepositoryImpl>();
        // services.AddScoped<IMarcaRepository, MarcaRepositoryImpl>();

        //En este caso hemos comentado este codigo porque ya no lo necesitamos,
        // ya que hemos implementado el patron de diseño UnitOfWork quien se encarga de instanciar los repositorios
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigurarLimitesDePeticiones(this IServiceCollection services) {

        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        services.Configure<IpRateLimitOptions>(options => {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";

            options.GeneralRules = new List<RateLimitRule> {
                new RateLimitRule {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 2,
                },
                new RateLimitRule {
                    Endpoint = "*",
                    Period = "1m",
                    Limit = 10,
                },
            };

            /*
                Ejemplo de Comportamiento Combinado:
                Si un usuario hace 2 peticiones en los primeros 10 segundos, deberá esperar hasta que
                termine ese periodo de 10 segundos para hacer más peticiones.
                En total, el usuario no podrá hacer más de 10 peticiones en un minuto, incluso si respeta
                la regla de 2 peticiones cada 10 segundos.
            */

        });

    }

    public static void ConfigurarVersionamientoDeAPI(this IServiceCollection services) {

        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true; //esto es para que en la respuesta de la API se muestre la version de la API
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            // options.ApiVersionReader = new QueryStringApiVersionReader("v"); //leer la version query string, eje: /api/productos?v=1
            // options.ApiVersionReader = new HeaderApiVersionReader("api-version");//para leer la version de la API desde el header se usa esta linea eje: api-version: 1
            
            // Combinado: el cliente puede especificar la version de la API en la url, en el query string o en el header
            // options.ApiVersionReader = ApiVersionReader.Combine(
            //     new QueryStringApiVersionReader("v"),
            //     new HeaderApiVersionReader("api-version,
            //     new UrlSegmentApiVersionReader()
            //   );

        });

        //con este metodo se reemplaza la palabra v{version} en la url por la version de la API 
        // y sustituye la version en la url quedando algo asi: https://localhost:5001/api/v1.0/productos en vez de https://localhost:5001/api/v{version}/productos
        services.AddVersionedApiExplorer(options => {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
