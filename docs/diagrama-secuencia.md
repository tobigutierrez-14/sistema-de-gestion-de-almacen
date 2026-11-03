# Diagrama de Secuencia: Registrar y Confirmar Venta

Este diagrama modela la interacción sincrónica entre la capa web y las entidades del dominio al confirmar una venta, actualizando las existencias del catálogo y el historial del cliente.

```mermaid
sequenceDiagram
    autonumber
    actor Usuario
    participant VentasController as Controlador (Ventas)
    participant Catalogo as CatalogoEnMemoria
    participant Venta as Venta (Dominio)
    participant Producto as Producto (Dominio)
    participant Cliente as Cliente (Dominio)

    Usuario ->> VentasController: POST /Ventas/Crear (ClienteId, MedioPagoId, Items)
    activate VentasController

    VentasController ->> Catalogo: ConfirmarVentaConMovimientos(venta)
    activate Catalogo

    loop Por cada detalle de la venta
        Catalogo ->> Producto: DisminuirStock(cantidad)
        activate Producto
        alt Stock suficiente
            Producto -->> Catalogo: Stock actualizado
        else Stock insuficiente
            Producto -->> Catalogo: Lanza InvalidOperationException
        end
        deactivate Producto
    end

    Catalogo -->> VentasController: Venta confirmada y movimientos registrados
    deactivate Catalogo

    VentasController ->> Cliente: registrarCompra(venta)
    activate Cliente
    Cliente -->> VentasController: Venta incorporada al historial
    deactivate Cliente

    VentasController -->> Usuario: Redirección /Ventas/Detalle/1 (Completada)
    deactivate VentasController
```