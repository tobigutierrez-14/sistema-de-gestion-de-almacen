# Diagrama de Clases del Dominio

```mermaid
classDiagram
    direction LR

    class Persona {
        <<abstract>>
        -int _id
        -string _nombre
        -string _telefono
        -string _email
        +getId() int
        +getNombre() string
        +getTelefono() string
        +getEmail() string
        +setNombre(string nuevoNombre) void
        +setTelefono(string nuevoTelefono) void
        +setEmail(string nuevoEmail) void
    }

    class Cliente {
        -string _dni
        -List~Venta~ _historialCompras
        +getDni() string
        +getHistorialCompras() IReadOnlyList~Venta~
        +registrarCompra(Venta venta) void
        +calcularTotalCompras() decimal
    }

    class Proveedor {
    }

    class Categoria {
        -int Id
        -string Nombre
        -string Descripcion
        +actualizarNombre(string nuevoNombre) void
        +actualizarDescripcion(string nuevaDescripcion) void
        +obtenerId() int
        +obtenerNombre() string
        +obtenerDescripcion() string
    }

    class Producto {
        -int Id
        -string Nombre
        -string Descripcion
        -decimal Precio
        -int Stock
        -int StockMinimo
        -Categoria Categoria
        +AumentarStock(int cantidad) void
        +DisminuirStock(int cantidad) void
        +HayStockDisponible(int cantidad) bool
        +NecesitaReposicion() bool
        +actualizarNombre(string nuevoNombre) void
        +actualizarDescripcion(string nuevaDescripcion) void
        +actualizarPrecio(decimal nuevoPrecio) void
        +actualizarStockMinimo(int nuevoStockMinimo) void
        +actualizarCategoria(Categoria nuevaCategoria) void
        +obtenerStock() int
        +obtenerPrecio() decimal
        +obtenerNombre() string
        +obtenerCategoria() Categoria
        +obtenerDescripcion() string
        +obtenerStockMinimo() int
    }

    class Venta {
        -int id
        -DateTime fecha
        -Cliente cliente
        -List~DetalleVenta~ detalles
        -MedioPago medioPago
        -EstadoVenta estado
        +AgregarDetalle(DetalleVenta detalle) void
        +CalcularTotal() decimal
        +ConfirmarVenta() void
        +CancelarVenta() void
        +ObtenerId() int
        +ObtenerFecha() DateTime
        +ObtenerCliente() Cliente
        +ObtenerMedioPago() MedioPago
        +ObtenerEstado() EstadoVenta
        +ObtenerDetalles() IReadOnlyList~DetalleVenta~
    }

    class DetalleVenta {
        -Producto _producto
        -int _cantidad
        -decimal _precioUnitario
        +getProducto() Producto
        +getCantidad() int
        +getPrecioUnitario() decimal
        +calcularSubtotal() decimal
    }

    class MedioPago {
        -int Id
        -TipoMedioPago Tipo
        -string Descripcion
        +actualizarDescripcion(string nuevaDescripcion) void
        +obtenerId() int
        +obtenerTipo() TipoMedioPago
        +obtenerDescripcion() string
    }

    class MovimientoStock {
        <<abstract>>
        -int _id
        -string _fecha
        -int _cantidad
        -string _motivo
        -Producto _producto
        +getId() int
        +getFecha() string
        +getCantidad() int
        +getMotivo() string
        +getProducto() Producto
        +getTipo() string*
        +Aplicar() void*
    }

    class IngresoStock {
        +getTipo() string
        +Aplicar() void
    }

    class EgresoStock {
        +getTipo() string
        +Aplicar() void
    }

    class EstadoVenta {
        <<enumeration>>
        Pendiente
        Completada
        Cancelada
    }

    class TipoMedioPago {
        <<enumeration>>
        Efectivo
        TarjetaCredito
        TarjetaDebito
        Transferencia
    }

    Persona <|-- Cliente
    Persona <|-- Proveedor
    MovimientoStock <|-- IngresoStock
    MovimientoStock <|-- EgresoStock

    Categoria "1" <-- "0..*" Producto : clasifica
    Cliente "1" <-- "0..*" Venta : comprador
    Cliente "1" --> "0..*" Venta : historialCompras
    MedioPago "1" <-- "0..*" Venta : pago
    Venta "1" *-- "0..*" DetalleVenta : detalles
    Producto "1" <-- "0..*" DetalleVenta : producto
    Producto "1" <-- "0..*" MovimientoStock : producto
    Venta --> EstadoVenta : estado
    MedioPago --> TipoMedioPago : tipo
