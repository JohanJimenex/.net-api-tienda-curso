# 📚 Resumen de lo aprendido y puntos importantes

1. 🗂️ **Patrón Repositorio e implementación de interfaces**
2. 🕹️ **Controladores**
   - 📄 Paginación
   - 🔍 QueryParams
3. 🏛️ **Arquitectura en capas (API, Core, Infrastructure)**
   - 📦 DTO, Entity
4. 🔧 **Inyección de dependencias (Extensión de servicio)**
5. 🌐 **Configurar CORS**
6. 🔄 **AutoMapper para mapear modelos/entidades**
7. 🛠️ **Patrón Unit of Work**
8. 🗃️ **Entity Framework Core (DbContext)**
   - 🐳 Docker: configurar un contenedor de base de datos usando una imagen de MySQL
   - 🏗️ Code First, migrar las entidades a la BD
   - 📏 Configurar las reglas y tablas en común con Fluent API
   - 🏷️ Annotations para agregar reglas a las propiedades de las entidades
9. 🔐 **Usar Authentication con JWT y proteger los endpoints con [Authorize] y con roles especificados en el token con [Authorize(Roles = "Empleado")]**
   - 🔑 Implementar JWT
10. 🆕 **Versionar APIs**
11. 🚦 **Limitar cantidad de peticiones al API**
12. 📋 **Manejar Loggers con Serilog**
13. ⚠️ **Gestión de errores y excepciones**
   - Creando una clase "ApiResponse" para enviar un estandar de mensajes de errores (Statuscode, Message)
   - Creando otra clase "ApiExcepcion" que hereda de "ApiRepsonse" pero con "Details"
     - Creando un midleware personalizado para atrapar todas las excepciones
     - 

## 🎁 Extras de Felipe Gavilan

1. 🗄️ **Output Cache**: Para guardar en cache las respuestas de las APIs por x tiempo para responder más rápido
2. 🔒 **Autenticación "con 2 líneas"**: Usando los paquetes Identity.FrameworkCore y Token de las últimas versiones de .NET

---

## Estructura de la Solución en .NET

Proyecto donde se implementa **CODE FIRST** y tambien **DATABASE FIRST** con EntityFramework
Tambien configurar las reglas de las tablas con FluentAPI

# Estructura de la Solución en .NET 

Esta solución está estructurada en tres capas principales: **API**, **Core**, e **Infraestructura**. Cada capa tiene una responsabilidad específica, lo que sigue los principios de la arquitectura limpia o en capas. Esto permite una separación clara de responsabilidades y facilita la mantenibilidad y escalabilidad del proyecto.

## Carpetas principales

### 1. API

La carpeta **API** contiene el proyecto que expone la funcionalidad de la aplicación a través de una **Web API**. Aquí es donde se definen los controladores que manejan las solicitudes HTTP. La API actúa como la interfaz pública de la aplicación y recibe las peticiones de los clientes (navegadores, aplicaciones móviles, etc.).

Responsabilidades de la capa API:

- Definir y exponer endpoints para la comunicación HTTP.
- Manejar las solicitudes y respuestas HTTP.
- Delegar la lógica de negocio a la capa **Core**.

### 2. Core

La carpeta **Core** contiene el núcleo de la aplicación. Aquí es donde se define toda la lógica de negocio y los modelos de dominio. La capa Core es **independiente** de la infraestructura, lo que significa que no debe tener ninguna dependencia en cómo se almacenan los datos o cómo se implementan los servicios externos.

Responsabilidades de la capa Core:

- Definir las **entidades** de dominio.
- Implementar la **lógica de negocio** de la aplicación.
- Declarar **interfaces** que serán implementadas por la capa de infraestructura.

### 3. Infraestructura

La carpeta **Infraestructura** es responsable de la implementación de cómo se interactúa con los sistemas externos, como bases de datos, servicios externos, y otros componentes de la infraestructura. Aquí se implementan las interfaces definidas en la capa **Core**, lo que permite desacoplar la lógica de negocio de los detalles técnicos de la infraestructura.

Responsabilidades de la capa Infraestructura:

- Implementar repositorios para el acceso a datos.
- Conectar con bases de datos, servicios en la nube, o APIs externas.
- Proveer implementaciones para las interfaces definidas en la capa **Core**.

## Flujo entre las capas

1. Un usuario realiza una solicitud HTTP a través de la API.
2. La **API** recibe la solicitud y la delega a un servicio en la capa **Core**.
3. La capa **Core** maneja la lógica de negocio y, si es necesario, interactúa con la capa de **Infraestructura** para obtener datos o conectarse a servicios externos.
4. La capa **Infraestructura** implementa las operaciones de acceso a datos o llamadas externas.
5. El resultado es devuelto a la **API**, que lo responde al usuario en formato adecuado (por ejemplo, JSON).

## Ventajas de esta arquitectura

- **Modularidad**: Cada capa tiene responsabilidades claramente definidas, lo que facilita el mantenimiento.
- **Desacoplamiento**: La lógica de negocio no depende de los detalles de la infraestructura.
- **Testabilidad**: Las capas son más fáciles de probar de manera aislada.
- **Escalabilidad**: La arquitectura permite que cada capa crezca y se modifique independientemente de las otras.

Mis notas:

Notas del patron de arquitectura de carpetas:

La capa "CORE" no va a depender de ningún proyecto
Pero la capa "Infraestructura" si dependerá de la "Core" por lo que hay que agregar la referencia de "CORE" al .csproj de "CORE" para que desde "Infraestructura" pueda acceder a los script de "CORE".

El proyecto "API" tendrá acceso al proyecto "Infraestructura"

# Diagrama de Arquitectura de Carpetas

```plaintext
+-------------------+
|       API         |
+-------------------+
          |
          v
+-------------------+
| Infraestructura   |
+-------------------+
          |
          v
+-------------------+
|       CORE        |
+-------------------+
```

Dentro de la capa "Infraestructura" se instalaron dos paquetes:

```shell
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" /> #Agregar en el proyecto a usar
    # Este paquete es para hacer la migracion desde la base de datos a modelos/clases (Database FIRST)
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.10" />
    # Este paquete es el proveedor de mhySQL para entity framework para usar el dbcontext 
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
```

**CODE FIRST**
1ro creamos los modelos
2do creamod una clase DbContext con la configuracion de las clases a migrar (ver TiendaContext.cs)

Con el comando :

```shell
    dotnet ef migrations add InitialCreate -p ./Infrastructure -s ./API -o Data/Migrations
```

Preparamos los archivos para la migracionde los modelos hacia la DB,

-p o --project, se indica la carpeta que contiene el DBContext
con -s o --startup se especifica el projecto de inicio , o sea, el que inicia la aplicaicon que es API que contiene el Program.cs

luego ejecutamos este comando para actualizar la base de datos
nota: la base de datos debe estar corriendo ya que se conectara a traves del connection string colcoado en el archivo appsetting.json y configurado en el Program.cs

```shell
    dotnet ef database update -p ./Infrastructure -s ./API
```

Otra opcion para que se haga la el update automatico es colocar este codigo en el program.cs

```csharp
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
```

Este es el codigo de configuracion:

```csharp
//  Program.cs
builder.Services.AddDbContext<TiendaContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
```

**DATABASE FIRST**

1ro. Creamos la base de datos y las tablas en la DB
2do. Ejecutamos el comando:
```shell
    # Este es para MySQL   
    dotnet ef dbcontext scafold "Server=localhost;User=root;Password=123456;Database=nombreDB" Pomelo.EntityFrameworkCore.Mysql -s NombreCarpetaDelProyectoEjemploAPI -p NombreCarpetadondeSeCreaContextDBEjemploInfrastructure --context NombreDelArchivoContext --context-dir NombreCarpetaEjmData --output-dir Entities
    
   
```



## Libreria/Dependencias/Paquetes/Nugets:

1. EntityFrameWorkCore /.tools /.Design / .mySql: ORM para interactuar con la DB 
1. CsvHelper : para leer archivos .CVS con datos y mandarlo a la base de datos
1. 
1. 
