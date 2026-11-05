# Sprint 4 — Recorridos de colecciones (T36–T39)

Implementación aislada en `Services/ConsultasColecciones.cs`, sin alterar las clases de dominio, controladores, vistas ni servicios existentes.

| Tarjeta | Operación LINQ | Descripción |
| --- | --- | --- |
| T36 Transformar colección de productos | `Select` | Proyecta `Producto` a `ResumenProducto`, con nombre, categoría, precio, stock y alerta de reposición. |
| T37 Encontrar primer producto con stock crítico | `FirstOrDefault` | Devuelve el primer producto cuyo `NecesitaReposicion()` es verdadero; `null` cuando no hay coincidencias. |
| T38 Contabilizar ventas por categoría | `Where`, `SelectMany`, `DistinctBy`, `GroupBy`, `Count` | Cuenta ventas **confirmadas** por categoría. Una venta cuenta una sola vez por categoría aunque tenga varios detalles de esa categoría. Clave: ID de categoría; valor: cantidad de ventas. |
| T39 Ordenar productos por ventas | `GroupBy`, `ToDictionary`, `OrderByDescending`, `ThenBy` | Clasifica todos los productos por **unidades vendidas** (descendente).

## Archivos para integrar

Nuevos (solo estos tres):
- `src/SistemaGestion.Web/Services/ConsultasColecciones.cs`
- `tests/SistemaGestion.Tests/ConsultasColeccionesTests.cs`
- `docs/SPRINT-4-RECORRIDOS.md`

**Archivos existentes modificados:** ninguno.

## Pruebas

```bash
dotnet build
dotnet test
```

Ocho pruebas nuevas cubren transformaciones y colección vacía; primer resultado y ausencia; deduplicación por categoría y ventas pendientes; orden por unidades, productos sin ventas y empates.

## Observaciones

- Las consultas aceptan colecciones del dominio y retornan resultados sin modificar las ventas ni el inventario.
- No se añadió una vista: las cuatro tarjetas son requisitos de programación sobre colecciones, demostrables por pruebas. Para visualizarlas en la web se requeriría conectarlas a un controlador y una vista.
- En este entorno no hay `dotnet` para ejecutar compilación o tests; verificar localmente y en CI.
