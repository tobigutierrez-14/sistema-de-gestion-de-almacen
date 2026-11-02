## Ejecutar

```sh
dotnet restore SistemaGestion.slnx
dotnet build SistemaGestion.slnx
dotnet test SistemaGestion.slnx
cd src/SistemaGestion.Web
dotnet run
```

Abrir `/Categorias`. Permite crear, editar, buscar y detectar nombres duplicados (sin distinguir mayúsculas). Los productos utilizan la misma instancia de `Categoria`, así que un cambio de nombre aparece inmediatamente en `/Productos` y en el selector de su formulario.

## Comprobación manual
1. Crear la categoría `Congelados`.
2. Comprobar su aparición en la selección de categoría de `Productos > Nuevo producto`.
3. Asignar un producto a `Congelados`.
4. Cambiar su nombre por `Productos congelados` y verificar el cambio en Productos.
5. Intentar registrar de nuevo el mismo nombre: debe aparecer un error.
6. Reiniciar la app: el estado vuelve a los datos de ejemplo, porque todavía no hay base de datos.

La eliminación de categorías queda fuera de esta tarea para evitar dejar productos sin categoría. No se modifican los modelos `Categoria` ni `Producto`.
