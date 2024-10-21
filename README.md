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

Dentro de la capa "Infraestructura"  se instalaron dos paquetes:
```shell
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
    # Este paquete es el proveedor de mhySQL para entity framework
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
```


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

Este es el codigo de configuracion:

```csharp
//  Program.cs
builder.Services.AddDbContext<TiendaContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
```

Otra opcion para que s ehaga automatica es colcoar este codigo en el program.cs

```csharp

```
