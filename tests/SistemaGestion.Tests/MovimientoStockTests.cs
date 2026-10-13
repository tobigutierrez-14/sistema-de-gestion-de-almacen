using System;
using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests;

public class MovimientoStockTests
{
    private Producto CrearProductoAuxiliar()
    {
        var categoria = new Categoria(1, "Almacén", "Comestibles");
        return new Producto(10, "Fideos 500g", "Fideos secos", 800m, 30, 5, categoria);
    }

    [Fact]
    public void CrearMovimientoStock_ConDatosValidos_RegistraCorrectamente()
    {
        // Caso Normal
        var producto = CrearProductoAuxiliar();
        string fechaFijada = "2026-10-12";

        var movimiento = new MovimientoStock(producto, "INGRESO", 20, "Recepción mercadería", fechaFijada);

        Assert.True(movimiento.getId() > 0);
        Assert.Equal(producto, movimiento.getProducto());
        Assert.Equal("INGRESO", movimiento.getTipo());
        Assert.Equal(20, movimiento.getCantidad());
        Assert.Equal("Recepción mercadería", movimiento.getMotivo());
        Assert.Equal("2026-10-12", movimiento.getFecha());
    }

    [Fact]
    public void CrearMovimientoStock_TipoConEspaciosYMinusculas_NormalizaAMayusculas()
    {
        // Caso Borde (tolerancia de formato en tipo)
        var producto = CrearProductoAuxiliar();

        var movimiento = new MovimientoStock(producto, "  egreso  ", 5, "Venta mostrador");

        Assert.Equal("EGRESO", movimiento.getTipo());
    }

    [Fact]
    public void CrearMovimientoStock_ConDatosInvalidos_LanzaExcepcion()
    {
        // Caso de Error
        var producto = CrearProductoAuxiliar();

        Assert.Throws<ArgumentNullException>(() => new MovimientoStock(null!, "INGRESO", 10, "Ajuste"));
        Assert.Throws<ArgumentException>(() => new MovimientoStock(producto, "TIPO_INVALIDO", 10, "Ajuste"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MovimientoStock(producto, "INGRESO", 0, "Ajuste"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MovimientoStock(producto, "INGRESO", -5, "Ajuste"));
        Assert.Throws<ArgumentException>(() => new MovimientoStock(producto, "INGRESO", 10, "   "));
    }
}