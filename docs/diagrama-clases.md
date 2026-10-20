# Diagrama de Clases del Dominio

```mermaid
classDiagram
    class Persona {
        <<abstract>>
        -int id
        -string nombre
        -string telefono
        -string email
        +getId() int
        +getNombre() string
        +getTelefono() string
        +getEmail() string
        +setNombre(string nuevoNombre) void
        +setTelefono(string nuevoTelefono) void
        +setEmail(string nuevoEmail) void
    }

    class Cliente {
        -string dni
        -List~Venta~ historialCompras
        +getDni() string
        +getHistorialCompras() List~Venta~
        +registrarCompra(Venta venta) void
        +calcularTotalCompras() decimal
    }

    class Proveedor {
    }

    Persona <|-- Cliente
    Persona <|-- Proveedor

    class Categoria {
        -int id
        -string nombre
        -string descripcion
        +actualizarNombre(string nuevoNombre) void
        +actualizarDescripcion(string nuevaDescripcion) void
        +obtenerId() int
        +obtenerNombre() string
        +obtenerDescripcion() string
    }

    class Producto {
        -int id
        -string nombre
        -string descripcion
        -decimal precio
        -int stock
        -int stockMinimo
        -Categoria categoria
        +AumentarStock(int cantidad) void
        +DisminuirStock(int cantidad) void
        +HayStockDisponible(int cantidad) bool
        +NecesitaReposicion() bool
        +actualizarPrecio(decimal nuevoPrecio) void
        +actualizarNombre(string nuevoNombre) void
        +actualizarDescripcion(string nuevaDescripcion) void
        +actualizarStockMinimo(int nuevoStockMinimo) void
        +actualizarCategoria(Categoria nuevaCategoria) void
        +obtenerStock() int
        +obtenerPrecio() decimal
        +obtenerNombre() string
        +obtenerCategoria() Categoria
    }

    class MovimientoStock {
        <<abstract>>
        -int id
        -string fecha
        -int cantidad
        -string motivo
        -Producto producto
        +getId() int
        +getFecha() string
        +getCantidad() int
        +getMotivo() string
        +getProducto() Producto
        +getTipo()* string
        +Aplicar()* void
    }

    class IngresoStock {
        +getTipo() string
        +Aplicar() void
    }

    class EgresoStock {
        +getTipo() string
        +Aplicar() void
    }

    MovimientoStock <|-- IngresoStock
    MovimientoStock <|-- EgresoStock

    class TipoMedioPago {
        <<enumeration>>
        EFECTIVO
        TARJETA_DEBITO
        TARJETA_CREDITO
        TRANSFERENCIA
    }

    class EstadoVenta {
        <<enumeration>>
        PENDIENTE
        CONFIRMADA
        CANCELADA
    }

    class MedioPago {
        -int id
        -TipoMedioPago tipo
        -string descripcion
        +actualizarDescripcion(string nuevaDescripcion) void
        +obtenerId() int
        +obtenerTipo() TipoMedioPago
        +obtenerDescripcion() string
    }

    class Venta {
        -int id
        -DateTime fecha
        -Cliente cliente
        -List~DetalleVenta~ detalles
        -MedioPago medioPago
        -EstadoVenta estado
        +agregarDetalle(DetalleVenta detalle) void
        +calcularTotal() decimal
        +confirmarVenta() void
        +cancelarVenta() void
        +obtenerDetalles() List~DetalleVenta~
        +obtenerEstado() EstadoVenta
        +obtenerCliente() Cliente
    }

    class DetalleVenta {
        -Producto producto
        -int cantidad
        -decimal precioUnitario
        +calcularSubtotal() decimal
    }

    Categoria "1" --> "0..*" Producto
    Cliente "1" --> "0..*" Venta
    Producto "1" <-- "0..*" DetalleVenta
    Producto "1" <-- "0..*" MovimientoStock
    Venta "1" *-- "1..*" DetalleVenta
    Venta "0..*" --> "1" MedioPago
    MedioPago --> TipoMedioPago
    Venta --> EstadoVenta
```