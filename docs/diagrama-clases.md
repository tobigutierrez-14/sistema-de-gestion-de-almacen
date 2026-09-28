
# Diagrama de Clases del Dominio

```mermaid
classDiagram
    class Categoria {
        -int id
        -string nombre
        -string descripcion
        +agregarProducto(Producto producto)
        +quitarProducto(Producto producto)
    }

    class Proveedor {
        -int id
        -string nombre
        -string telefono
        -string email
    }

    class Cliente {
        -int id
        -string nombre
        -string apellido
        -string telefono
        -string email
        -List~Venta~ historial
        +registrarCompra(Venta venta) void
        +obtenerHistorial() List~Venta~
        +calcularTotalComprado() double
    }

    class Producto {
        -int id
        -string nombre
        -double precio
        -int stock
        -int stocMinimo
        -Categoria categoria
        +aumentarStock(int cantidad)
        +disminuirStock(int cantidad)
        +necesitaReposicion() bool
        +actualizarPrecio(double nuevoPrecio)
    }

    class Venta {
        -int id
        -string fecha
        -Cliente cliente
        -List~DetalleVenta~ detalles
        -MedioPago medioPago
        +agregarDetalle(DetalleVenta detalle)
        +calcularTotal() decimal
        +confirmarVenta()
    }

    class DetalleVenta {
        -Producto producto
        -int cantidad
        -double precioUnitario
        +calcularSubtotal() double
    }

    class MovimientoStock {
        -int id
        -string fecha
        -string tipo
        -int cantidad
        -string motivo
        -Producto producto
    }

    class MedioPago {
        -int id
        -string nombre
    }

    Categoria "1" --> "0..*" Producto
    Proveedor "1" --> "0..*" Producto
    Cliente "1" --> "0..*" Venta
    Producto "1" <-- "0..*" DetalleVenta
    Producto "1" <-- "0..*" MovimientoStock
    Venta "1" *-- "1..*" DetalleVenta
    Venta "0..*" --> "1" MedioPago
```