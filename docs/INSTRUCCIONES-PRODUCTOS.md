# Etapa 2: Productos

## Iniciar

Requiere SDK .NET 10.

```bash
cd src/SistemaGestion.Web
dotnet run
```

Entrar a la URL que indica la consola y hacer clic en **Productos** en el menú lateral. También se puede acceder mediante `/Productos`.

## Qué permite hacer

- Listar los productos cargados inicialmente como demostración.
- Filtrar por nombre/descripción, categoría y estado de stock.
- Registrar productos desde **Nuevo producto**.
- Editar productos desde el enlace **Editar**.
- Validar nombre, categoría, precio y cantidades en el servidor.
- Señalar stock bajo cuando `Stock <= StockMinimo`.
- Reutilizar la clase `Producto` original del proyecto y su lógica de stock.

## Limitaciones de esta etapa

Los registros viven en un servicio singleton en memoria; al detener y volver a iniciar la aplicación se restablecen los productos de muestra. Si varios usuarios entran, verán la misma lista en memoria. Todavía no hay base de datos ni autenticación. El dashboard continúa con indicadores ilustrativos, independientes del catálogo.

## Archivos nuevos

- `Services/CatalogoEnMemoria.cs`
- `Controllers/ProductosController.cs`
- `ViewModels/ProductoFormViewModel.cs`
- `ViewModels/ProductosIndexViewModel.cs`
- `Views/Productos/Index.cshtml`
- `Views/Productos/Formulario.cshtml`

Los archivos originales `Models` y `tests` permanecen intactos.

## Pruebas manuales recomendadas

1. Crear un producto y comprobar que aparece en el listado.
2. Buscarlo por parte de su nombre.
3. Filtrar por su categoría.
4. Editar el precio y la cantidad de stock.
5. Poner stock inferior o igual al mínimo para activar el estado “Reponer”.
6. Intentar guardar valores negativos y verificar que se rechazan.