# Etapa 1 — Dashboard y menú lateral

Este paquete conserva los modelos y pruebas del proyecto original, e incorpora MVC, un layout responsive y páginas provisionales de navegación. Todos los indicadores son **datos de ejemplo**; aún no hay persistencia.

## Requisitos
- SDK .NET 10

## Ejecutar
```bash
cd src/SistemaGestion.Web
dotnet run
```
Abrí la URL local que aparezca en la consola. Si necesitás ejecutar las pruebas, desde la raíz: `dotnet test`.

## Archivos principales
- `Program.cs`: configura MVC y archivos estáticos.
- `Views/Shared/_Layout.cshtml`: menú lateral y cabecera reutilizables.
- `Views/Home/Index.cshtml`: dashboard.
- `ViewModels/DashboardViewModel.cs`: datos ilustrativos.
- `Controllers/HomeController.cs`: carga el dashboard.
- `Controllers/SeccionesController.cs` y `Views/Secciones/Index.cshtml`: páginas temporales para las secciones futuras.
- `wwwroot/css/site.css`: estilos responsive.
- `wwwroot/js/site.js`: apertura/cierre del menú móvil.

## Próxima etapa
Reemplazar la página temporal de Productos por listado y modal de alta, usando un repositorio en memoria hasta incorporar base de datos.
