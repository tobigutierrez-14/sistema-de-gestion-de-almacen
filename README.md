# Sistema de Gestion de Almacen

Sistema de gestión para un almacén natural y dietética, desarrollado como trabajo integrador de Metodologías de Programación II (UNAJ).

---

## Objetivo del proyecto

El sistema busca facilitar la gestión de productos, categorías, stock, clientes y ventas, reduciendo el registro manual de información y permitiendo consultar datos relevantes para el funcionamiento del negocio.

---

## Tecnologías

- Lenguaje: C#
- Framework: ASP.NET Core
- Pruebas automatizadas: xUnit
- Control de versiones: Git y GitHub
- Entorno de desarrollo: Visual Studio Code

---

## Estructura general del proyecto

```text
sistema-de-gestion-de-almacen/
├── .github/
│   └── workflows/
│       └── ci.yml
├── docs/
│   ├── diagrama-clases.md
│   ├── diagrama-secuencia.md
│   ├── INSTRUCCIONES-CATEGORIAS.md
│   ├── INSTRUCCIONES-CLIENTES.md
│   ├── INSTRUCCIONES-DASHBOARD.md
│   ├── INSTRUCCIONES-PRODUCTOS.md
│   ├── INSTRUCCIONES-STOCK.md
│   ├── INSTRUCCIONES-VENTAS.md
│   └── SPRINT-4-RECORRIDOS.md
├── src/
│   └── SistemaGestion.Web/
│       ├── Controllers/
│       │   ├── CategoriasController.cs
│       │   ├── ClientesController.cs
│       │   ├── HomeController.cs
│       │   ├── ProductosController.cs
│       │   ├── ProveedoresController.cs
│       │   ├── SeccionesController.cs
│       │   ├── StockController.cs
│       │   └── VentasController.cs
│       ├── Models/
│       │   ├── Persona.cs
│       │   ├── Cliente.cs
│       │   ├── Proveedor.cs
│       │   ├── Producto.cs
│       │   ├── Categoria.cs
│       │   ├── Venta.cs
│       │   ├── DetalleVenta.cs
│       │   ├── EstadoVenta.cs
│       │   ├── MedioPago.cs
│       │   ├── TipoMedioPago.cs
│       │   ├── MovimientoStock.cs
│       │   ├── IngresoStock.cs
│       │   └── EgresoStock.cs
│       ├── Services/
│       │   ├── CatalogoEnMemoria.cs
│       │   ├── ClientesEnMemoria.cs
│       │   ├── ConsultasColecciones.cs
│       │   └── VentasEnMemoria.cs
│       ├── ViewModels/
│       │   ├── CategoriaFilaViewModel.cs
│       │   ├── CategoriaFormViewModel.cs
│       │   ├── ClienteViewModels.cs
│       │   ├── DashboardViewModel.cs
│       │   ├── ProductoFormViewModel.cs
│       │   ├── ProductoViewModel.cs
│       │   ├── StockViewModels.cs
│       │   └── VentasViewModels.cs
│       ├── Views/
│       │   ├── Categorias/
│       │   ├── Clientes/
│       │   ├── Home/
│       │   ├── Productos/
│       │   ├── Secciones/
│       │   ├── Shared/
│       │   ├── Stock/
│       │   └── Ventas/
│       ├── wwwroot/
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── Program.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       └── SistemaGestion.Web.csproj
├── tests/
│   └── SistemaGestion.Tests/
│       ├── ClienteTests.cs
│       ├── ProveedorTests.cs
│       ├── ProductoTests.cs
│       ├── CategoriaTests.cs
│       ├── MedioPagoTests.cs
│       ├── MovimientoStockTests.cs
│       └── SistemaGestion.Tests.csproj
├── SistemaGestion.slnx
├── .gitignore
└── README.md
```

Nota: la estructura muestra la organización prevista del modelo. Algunos archivos y funcionalidades pueden estar pendientes de implementación o integración.

---

## Descripción de las carpetas

- `src/SistemaGestion.Web`: contiene el proyecto principal de la aplicación web, incluyendo controladores, modelos, servicios de persistencia en memoria, vistas Razor y archivos estáticos.
- `src/SistemaGestion.Web/Models`: contiene las clases del modelo de negocio, sus atributos, métodos, validaciones y reglas.
- `tests/SistemaGestion.Tests`: contiene las pruebas unitarias automatizadas de las clases implementadas.
- `docs`: contiene la documentación del proyecto, guías funcionales e instrucciones por módulo, junto a los diagramas de clases y de secuencia en Mermaid.
- `Program.cs`: configura e inicia la aplicación ASP.NET Core.
- `SistemaGestion.slnx`: agrupa los proyectos de la solución.
- `README.md`: contiene la información general y las instrucciones del proyecto.
- `.github/workflows`: contiene el flujo de Integración Continua (CI) que compila y corre las pruebas automáticamente.

Program.cs: configura e inicia l

---

## Clases principales

- **Producto:** representa un artículo comercializado y controla su precio y stock.
- **Categoria:** permite clasificar los productos.
- **Venta:** representa una operación de venta y coordina sus detalles.
- **DetalleVenta:** representa un producto y su cantidad dentro de una venta.
- **Persona:** superclase base abstracta que encapsula la identidad y datos de contacto comunes.
- **Cliente:** hereda de Persona, almacena la información de los clientes y mantiene su historial de compras.
- **Proveedor:** hereda de Persona y modela los proveedores del almacén.
- **MovimientoStock:** clase abstracta para la gestión del inventario, especializada en **IngresoStock** y **EgresoStock**.
- **MedioPago:** representa el medio de pago utilizado en una venta.
- **TipoMedioPago:** enumera los medios de pago disponibles.
- **EstadoVenta:** representa los posibles estados de una venta.

Las clases se desarrollan aplicando programación orientada a objetos, herencia, polimorfismo, encapsulación y validaciones de las reglas del negocio.

---

## Requisitos previos

Para ejecutar el proyecto se necesita:

- .NET SDK compatible con el framework definido en SistemaGestion.Web.csproj.
- Git.
- Visual Studio Code u otro editor compatible con C#.

---

## Integrantes

- Ruiz, Gonzalo Rodrigo - Comisión 1
- Gutierrez, Tobias Jonas - Comisión 1

---

## Tecnologías y Metodología

- Metodología: Scrum (Tablero en Trello)
- Lenguaje: C# (.NET)
- Interfaz: ASP.NET Core (Web sencilla)

---

## Ejecución y Pruebas

- Para ejecutar el proyecto web: `dotnet run --project src/SistemaGestion.Web`
- Para ejecutar las pruebas unitarias: `dotnet test`

Las pruebas permiten verificar el comportamiento de las clases implementadas y sus validaciones.

---

## Control de versiones y trabajo colaborativo

- El proyecto utiliza Git y GitHub para organizar el desarrollo colaborativo.
- Las funcionalidades se desarrollan mediante ramas de trabajo.
- Los commits deben tener mensajes descriptivos siguiendo la convención de Conventional Commits.
- Los cambios se integran de acuerdo con el flujo de trabajo acordado por el equipo mediante Pull Requests.
- Las pruebas se ejecutan para verificar los cambios antes de integrarlos.
- Integración Continua (CI): cada Pull Request y push a la rama principal dispara automáticamente la compilación y ejecución de la suite de pruebas mediante GitHub Actions antes de integrarse.

---

## Estado del proyecto

El proyecto ha finalizado con éxito su alcance previsto, cumpliendo con la totalidad de los requisitos, entidades y funcionalidades comprometidas en la propuesta inicial aprobada. 

La arquitectura implementada en capas y el diseño orientado a objetos con bajo acoplamiento garantizan la escalabilidad del sistema, permitiendo incorporar futuras extensiones (tales como persistencia en base de datos relacional, pasarelas de pago externas o nuevos módulos de gestión) de manera ágil y sostenible.