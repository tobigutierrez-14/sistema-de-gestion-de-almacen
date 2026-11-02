# T45 — Módulo Ventas: implementación e instrucciones

## Objetivo

La sección **Ventas** del Sistema de Gestión de Almacén permite registrar y consultar ventas en ASP.NET Core MVC. La pantalla **Nueva venta** incluye un **buscador de productos por nombre o categoría**, un **carrito dinámico** con cantidades editables y un **resumen lateral** del importe estimado.

La lógica de negocio se conserva en C#: `Venta`, `DetalleVenta`, `Producto`, `Cliente` y los servicios del proyecto. El JavaScript solo administra la interfaz y calcula importes orientativos; el servidor valida las existencias y calcula el importe definitivo al confirmar.

## Estado de la funcionalidad

- Consultar listado e historial de ventas y abrir el detalle de cada operación.
- Elegir cliente y medio de pago.
- Filtrar productos por nombre o categoría.
- Agregar productos al carrito, modificar cantidades y quitar productos.
- Acumular cantidades si se agrega nuevamente el mismo producto.
- Consultar subtotales, número de productos, unidades y total estimado.
- Confirmar la venta validando productos, cantidades y stock en servidor.
- Descontar existencias, registrar egresos de Stock y asociar la compra al cliente.
- Reflejar ventas e ingresos del día en Dashboard.

No existe ya el límite visual fijo de cinco filas: el carrito genera dinámicamente las líneas enviadas al servidor. Continúan pendientes la persistencia en una base de datos y el flujo de anulación de ventas ya confirmadas.

### Archivos incorporados al crear el módulo

- `src/SistemaGestion.Web/Controllers/VentasController.cs` — acciones de listado, alta y detalle.
- `src/SistemaGestion.Web/Services/VentasEnMemoria.cs` — coordinación de las ventas en memoria.
- `src/SistemaGestion.Web/ViewModels/VentasViewModels.cs` — modelos de formulario y listado.
- `src/SistemaGestion.Web/Views/Ventas/Index.cshtml` — historial.
- `src/SistemaGestion.Web/Views/Ventas/Crear.cshtml` — nueva venta.
- `src/SistemaGestion.Web/Views/Ventas/Detalle.cshtml` — detalle de una venta.

### Archivos modificados al crear el módulo

- `src/SistemaGestion.Web/Services/CatalogoEnMemoria.cs` — integración con Producto y registro de egresos.
- `src/SistemaGestion.Web/Services/ClientesEnMemoria.cs` — obtención de clientes y registro de sus compras.
- `src/SistemaGestion.Web/ViewModels/ClienteViewModels.cs` — contacto opcional en el formulario de Clientes.
- `src/SistemaGestion.Web/Program.cs` — registro de `VentasEnMemoria` en la inyección de dependencias.
- `src/SistemaGestion.Web/Controllers/HomeController.cs` — indicadores de ventas.
- `src/SistemaGestion.Web/ViewModels/DashboardViewModel.cs` — campos del resumen del día.
- `src/SistemaGestion.Web/Views/Home/Index.cshtml` — tarjetas y accesos.
- `src/SistemaGestion.Web/Views/Shared/_Layout.cshtml` — navegación a `VentasController`.
- `src/SistemaGestion.Web/wwwroot/css/site.css` — estilos de ventas.

## Flujo de registro

1. Abrir **Ventas → Nueva venta** (`/Ventas/Crear`).
2. Seleccionar el cliente y el medio de pago.
3. Buscar un producto por nombre o categoría.
4. Elegir la cantidad y agregar el producto al carrito.
5. Cambiar cantidades o quitar productos, si hace falta.
6. Revisar el resumen lateral y confirmar la venta.
7. El formulario envía los elementos como `Lineas[0].ProductoId`, `Lineas[0].Cantidad`, etc., que son interpretados por el controlador MVC.
8. El servidor vuelve a verificar los datos, toma los precios vigentes, calcula el total y confirma la venta.
9. La venta confirmada se refleja en Stock, el historial del cliente y los indicadores del Dashboard.

**Importante:** el precio, el stock y los importes calculados por JavaScript son informativos; no se deben aceptar como fuente de verdad en el servidor.

## Integración con Stock y clientes

`Venta.ConfirmarVenta()` ya descuenta el stock a través de los modelos existentes. Al registrar los movimientos `EgresoStock` asociados a la venta, **no volver a ejecutar `Aplicar()`**, para evitar descontar las unidades dos veces. Los cambios manuales de stock siguen gestionándose con las subclases `IngresoStock` y `EgresoStock`, según la implementación del módulo Stock.

Los clientes se gestionan desde `ClientesEnMemoria`, y la venta confirmada se asocia con la instancia del cliente correspondiente. La interfaz de Clientes mantiene teléfono y correo como campos opcionales.

## Compilación y pruebas

Desde la raíz de la solución:

```bash
dotnet restore
dotnet build
dotnet test
```

Ejecutar la web:

```bash
cd src/SistemaGestion.Web
dotnet run
```

### Verificaciones manuales recomendadas

1. Crear o seleccionar un cliente y abrir `/Ventas/Crear`.
2. Buscar un producto por su nombre y después por su categoría: confirmar que el catálogo se filtra.
3. Agregar dos productos diferentes: comprobar que aparecen en el carrito.
4. Volver a agregar uno de ellos: comprobar que se acumulan las unidades, sin duplicar el producto.
5. Modificar la cantidad y eliminar un producto: verificar subtotales y total estimado.
6. Intentar una cantidad superior al stock o una cantidad inválida: comprobar que se impide o rechaza correctamente la operación.
7. Confirmar una venta válida: consultar detalle, historial, existencias y movimientos de Stock.
8. Revisar que el historial de compras del cliente y el Dashboard reflejen la operación.
9. Comprobar en móvil que la vista apila los paneles y no presenta scroll horizontal.
10. Ejecutar CI en GitHub Actions antes de integrar el Pull Request.

## Limitaciones actuales

- **Persistencia en memoria:** los registros se pierden al reiniciar la aplicación.
- **Confirmación inmediata:** todavía no existe una gestión completa de borradores y cancelaciones de ventas confirmadas.
- **Total estimado:** la interfaz muestra un cálculo previo, pero el importe definitivo se determina en el servidor.
- **Operaciones transaccionales:** para garantizar consistencia entre ventas, inventario e historial ante fallos y despliegues con múltiples instancias se requerirá persistencia y una unidad de trabajo transaccional.