using System;
using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests;

public class DetalleVentaTests
{
    private Producto CrearProductoAuxiliar(decimal precio = 150m, int stock = 20)
    {
        var categoria = new Categoria(1, "Almacén", "Productos generales");
        return new Producto(1, "Yerba Mate 1kg", "Yerba clásica", precio, stock, 5, categoria);
    }

    [Fact]
    public void CalcularSubtotal_ConValoresValidos_CalculaCorrectamente()
    {
        // Caso Normal
        var producto = CrearProductoAuxiliar(150.50m);
        var detalle = new DetalleVenta(producto, 3, 150.50m);

        decimal subtotal = detalle.calcularSubtotal();

        Assert.Equal(451.50m, subtotal);
        Assert.Equal(producto, detalle.getProducto());
    }

    [Fact]
    public void CrearDetalleVenta_ConPrecioCero_CalculaSubtotalCero()
    {
        // Caso Borde (bonificación o muestra sin costo)
        var producto = CrearProductoAuxiliar(0m);
        var detalle = new DetalleVenta(producto, 5, 0m);

        decimal subtotal = detalle.calcularSubtotal();

        Assert.Equal(0m, subtotal);
    }

    [Fact]
    public void CrearDetalleVenta_ConDatosInvalidos_LanzaExcepcion()
    {
        // Caso de Error
        var producto = CrearProductoAuxiliar(100m);

        Assert.Throws<ArgumentNullException>(() => new DetalleVenta(null!, 1, 100m));
        Assert.Throws <ArgumentOutOfRangeException>(() => new DetalleVenta(producto, 0, 100m));
        Assert.Throws <ArgumentOutOfRangeException>(() => new DetalleVenta(producto, -2, 100m));
        Assert.Throws <ArgumentOutOfRangeException>(() => new DetalleVenta(producto, 1, -50m));
    }
}