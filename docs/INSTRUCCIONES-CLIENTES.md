# Vista Clientes

## Archivos nuevos
- `src/SistemaGestion.Web/Controllers/ClientesController.cs`
- `src/SistemaGestion.Web/Services/ClientesEnMemoria.cs`
- `src/SistemaGestion.Web/ViewModels/ClienteViewModels.cs`
- `src/SistemaGestion.Web/Views/Clientes/Index.cshtml`
- `src/SistemaGestion.Web/Views/Clientes/Formulario.cshtml`
- `tests/SistemaGestion.Tests/ClientesModuloTests.cs`
- `docs/INSTRUCCIONES-CLIENTES.md`

## Archivos modificados
- `src/SistemaGestion.Web/Program.cs`: registro del servicio `ClientesEnMemoria` en DI.
- `src/SistemaGestion.Web/Views/Shared/_Layout.cshtml`: ruta directa de Clientes y preservación del enlace corregido a Stock.

## Archivos NO modificados
- `Models/Persona.cs`, `Models/Cliente.cs`, `Services/CatalogoEnMemoria.cs`, las vistas y modelos de Stock, Productos y Categorías.

## Notas
- `Cliente` hereda de `Persona`; el ID es generado en la clase padre y el DNI no tiene setter. Por eso no se cambia el DNI en una edición.
- `getHistorialCompras` y `calcularTotalCompras` muestran 0 inicialmente; futuras ventas podrán agregarse mediante `registrarCompra`.
- Los clientes se guardan en memoria. Se pierden al reiniciar el servidor.
- Ejecutar desde la raíz: `dotnet build`, `dotnet test`; iniciar la web y abrir `/Clientes`.
