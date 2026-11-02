# Módulo de Stock

Abrir `/Stock`. Registrar un ingreso y un egreso; comprobar stock en `/Productos`, alertas y actividad en `/Dashboard`. Probar egreso superior al stock disponible y confirmar que no se registra movimiento. Editar el stock desde Productos y comprobar que aparece un ajuste en historial.

## Notas
- Solo nuevos movimientos y ajustes de edición tienen historial; el stock inicial y el stock de productos nuevos no generan movimientos históricos.
- Los datos y movimientos se reinician al detener la aplicación.
- No hay base de datos implementado en esta versión.
