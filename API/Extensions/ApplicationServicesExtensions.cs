using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
