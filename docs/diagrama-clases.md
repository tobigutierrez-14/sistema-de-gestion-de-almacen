\# Diagrama de Clases del Dominio



```mermaid

classDiagram

&#x20;   class Categoria {

&#x20;       -int id

&#x20;       -string nombre

&#x20;       -string descripcion

&#x20;       +agregarProducto(Producto producto)

&#x20;       +quitarProducto(Producto producto)

&#x20;   }



&#x20;   class Proveedor {

&#x20;       -int id

&#x20;       -string nombre

&#x20;       -string telefono

&#x20;       -string email

&#x20;   }



&#x20;   class Cliente {

&#x20;       -int id

&#x20;       -string nombre

&#x20;       -string apellido

&#x20;       -string telefono

&#x20;       -string email

&#x20;       -List\~Venta\~ historial

&#x20;       +registrarCompra(Venta venta) void

&#x20;       +obtenerHistorial() List\~Venta\~

&#x20;       +calcularTotalComprado() double

&#x20;   }



&#x20;   class Producto {

&#x20;       -int id

&#x20;       -string nombre

&#x20;       -double precio

&#x20;       -int stock

&#x20;       -int stocMinimo

&#x20;       -Categoria categoria

&#x20;       +aumentarStock(int cantidad)

&#x20;       +disminuirStock(int cantidad)

&#x20;       +necesitaReposicion() bool

&#x20;       +actualizarPrecio(double nuevoPrecio)

&#x20;   }



&#x20;   class Venta {

&#x20;       -int id

&#x20;       -string fecha

&#x20;       -Cliente cliente

&#x20;       -List\~DetalleVenta\~ detalles

&#x20;       -MedioPago medioPago

&#x20;       +agregarDetalle(DetalleVenta detalle)

&#x20;       +calcularTotal() decimal

&#x20;       +confirmarVenta()

&#x20;   }



&#x20;   class DetalleVenta {

&#x20;       -Producto producto

&#x20;       -int cantidad

&#x20;       -double precioUnitario

&#x20;       +calcularSubtotal() double

&#x20;   }



&#x20;   class MovimientoStock {

&#x20;       -int id

&#x20;       -string fecha

&#x20;       -string tipo

&#x20;       -int cantidad

&#x20;       -string motivo

&#x20;       -Producto producto

&#x20;   }



&#x20;   class MedioPago {

&#x20;       -int id

&#x20;       -string nombre

&#x20;   }



&#x20;   Categoria "1" --> "\*" Producto

&#x20;   Proveedor "1" --> "\*" Producto

&#x20;   Cliente "1" --> "\*" Venta

&#x20;   Producto "1" <-- "\*" DetalleVenta

&#x20;   Producto "1" <-- "\*" MovimientoStock

&#x20;   Venta "1" \*-- "\*" DetalleVenta

&#x20;   Venta "\*" --> "1" MedioPago

```

