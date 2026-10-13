using System;

namespace SistemaGestion.Web.Models;

public class DetalleVenta
{
    //Atributos
    private Producto _producto;
    private int _cantidad;
    private decimal _precioUnitario;

    public DetalleVenta(Producto producto, int cantidad, decimal precioUnitario)
    {
        if (producto == null)
            throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");
        
        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor que cero.");
       
        if (precioUnitario < 0)
            throw new ArgumentOutOfRangeException(nameof(precioUnitario), "El precio unitario no puede ser negativo.");

        _producto = producto;
        _cantidad = cantidad;
        _precioUnitario = precioUnitario;
    }

    //Getters
    public Producto getProducto() { return _producto; }
    public int getCantidad() { return _cantidad; }
    public decimal getPrecioUnitario() { return _precioUnitario; }

    //Metodos
    public decimal calcularSubtotal(){return _cantidad * _precioUnitario;}
}