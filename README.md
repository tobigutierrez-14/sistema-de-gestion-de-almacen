# Sistema de Gestion de Almacen

Sistema de gestión para un almacén natural y dietética, desarrollado como trabajo integrador de Metodologías de Programación II (UNAJ).

# Objetivo del proyecto

El sistema busca facilitar la gestión de productos, categorías, stock, clientes y ventas, reduciendo el registro manual de información y permitiendo consultar datos relevantes para el funcionamiento del negocio.

# Tecnologías

• Lenguaje: C#
• Framework: ASP.NET Core
• Pruebas automatizadas: xUnit
• Control de versiones: Git y GitHub
• Entorno de desarrollo: Visual Studio Code

# Estructura general del proyecto

sistema-de-gestion-de-almacen/
│
├── src/
│   └── SistemaGestion.Web/
│       ├── Models/
│       │   ├── Producto.cs
│       │   ├── Categoria.cs
│       │   ├── Venta.cs
│       │   ├── DetalleVenta.cs
│       │   ├── Cliente.cs
│       │   ├── MedioPago.cs
│       │   ├── TipoMedioPago.cs
│       │   └── EstadoVenta.cs
│       │
│       ├── Properties/
│       ├── Program.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       └── SistemaGestion.Web.csproj
│
├── tests/
│   └── SistemaGestion.Tests/
│       ├── ProductoTests.cs
│       ├── CategoriaTests.cs
│       └── MedioPagoTests.cs
│       
├── SistemaGestion.sln
├── .gitignore
└── README.md
Nota: la estructura muestra la organización prevista del modelo. Algunos archivos y funcionalidades pueden estar pendientes de implementación o integración.

# Descripción de las carpetas

• src/SistemaGestion.Web/: contiene el proyecto principal de la aplicación web.
• Models/: contiene las clases del modelo de negocio, sus atributos, métodos y reglas.
• tests/SistemaGestion.Tests/: contiene las pruebas automatizadas de las clases implementadas.
• Program.cs: configura e inicia la aplicación ASP.NET Core.
• SistemaGestion.sln: agrupa los proyectos de la solución.
• README.md: contiene la información general y las instrucciones del proyecto.

# Clases principales

• Producto: representa un artículo comercializado y controla su precio y stock.

• Categoria: permite clasificar los productos.
• Venta: representa una operación de venta y coordina sus detalles.
• DetalleVenta: representa un producto y su cantidad dentro de una venta.
• Cliente: almacena la información de los clientes.
• MedioPago: representa el medio de pago utilizado en una venta.
• TipoMedioPago: enumera los medios de pago disponibles.
• EstadoVenta: representa los posibles estados de una venta.

Las clases se desarrollan aplicando programación orientada a objetos, encapsulación y validaciones de las reglas del negocio.

# Requisitos previos

Para ejecutar el proyecto se necesita:

• .NET SDK compatible con el framework definido en SistemaGestion.Web.csproj.
• Git.
• Visual Studio Code u otro editor compatible con C#.

## Integrantes
- Ruiz, Gonzalo Rodrigo - Comision 1
- Gutierrez, Tobias Jonas - Comision 1

## Tecnologias y Metodologia
- **Metodologia:** Scrum (Tablero en Trello)
- **Lenguaje:** C# (.NET)
- **Interfaz:** ASP.NET Core (Web sencilla)

##  Ejecucion y Pruebas
- Para ejecutar el proyecto web: `dotnet run --project src/SistemaGestion.Web`
- Para ejecutar las pruebas unitarias: `dotnet test`
Las pruebas permiten verificar el comportamiento de las clases implementadas y sus validaciones.

# Control de versiones y trabajo colaborativo

• El proyecto utiliza Git y GitHub para organizar el desarrollo colaborativo.
• Las funcionalidades se desarrollan mediante ramas de trabajo.
• Los commits deben tener mensajes descriptivos.
• Los cambios se integran de acuerdo con el flujo de trabajo acordado por el equipo.
• Las pruebas se ejecutan para verificar los cambios antes de integrarlos.

# Estado del proyecto

El proyecto se encuentra en desarrollo. Las clases, pruebas y funcionalidades se incorporan progresivamente durante las iteraciones del trabajo integrador.

