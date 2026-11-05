using SistemaGestion.Web.Models;
using SistemaGestion.Web.Services;
using Xunit;

namespace SistemaGestion.Tests;

public class ConsultasColeccionesTests
{
    private static readonly Categoria Almacen = new(1, "Almacén", "Secos");
    private static readonly Categoria Bebidas = new(2, "Bebidas", "Líquidos");

    private static Producto Producto(int id, string nombre, int stock, int minimo, Categoria? categoria = null) =>
        new(id, nombre, "Descripción", 100m, stock, minimo, categoria ?? Almacen);

    private static Venta VentaConfirmada(int id, params (Producto Producto, int Cantidad)[] lineas)
    {
        var cliente = new Cliente("Prueba", $"{id + 1000}");
        var medio = new MedioPago(1, TipoMedioPago.Efectivo, "Efectivo");
        var venta = new Venta(id, DateTime.Today, cliente, medio);
        foreach (var (producto, cantidad) in lineas)
            venta.AgregarDetalle(new DetalleVenta(producto, cantidad, producto.obtenerPrecio()));
        venta.ConfirmarVenta();
        return venta;
    }

    [Fact]
    public void T36_TransformaProductosSinModificarOrigen()
    {
        var p = Producto(1, "Arroz", 3, 5);
        var lista = ConsultasColecciones.TransformarProductos([p]);
        Assert.Equal("Arroz", Assert.Single(lista).Nombre);
        Assert.True(lista[0].NecesitaReposicion);
        Assert.Equal(3, p.obtenerStock());
    }

    [Fact]
    public void T36_ColeccionVacia_ProduceColeccionVacia() =>
        Assert.Empty(ConsultasColecciones.TransformarProductos([]));

    [Fact]
    public void T37_DevuelvePrimerCriticoSegunOrden()
    {
        var normal = Producto(1, "Normal", 20, 5);
        var primero = Producto(2, "Primero", 5, 5);
        var segundo = Producto(3, "Segundo", 2, 5);
        Assert.Same(primero, ConsultasColecciones.PrimerProductoConStockCritico([normal, primero, segundo]));
    }

    [Fact]
    public void T37_SinCriticos_RetornaNull() =>
        Assert.Null(ConsultasColecciones.PrimerProductoConStockCritico([Producto(1, "Normal", 20, 5)]));

    [Fact]
    public void T38_CuentaUnaVentaPorCategoriaSinDuplicarLineas()
    {
        var arroz = Producto(1, "Arroz", 20, 2);
        var aceite = Producto(2, "Aceite", 20, 2);
        var agua = Producto(3, "Agua", 20, 2, Bebidas);
        var venta = VentaConfirmada(1, (arroz, 1), (aceite, 2), (agua, 1));
        var conteo = ConsultasColecciones.ContabilizarVentasPorCategoria([venta]);
        Assert.Equal(1, conteo.Single(x => x.Categoria == "Almacén").CantidadVentas);
        Assert.Equal(1, conteo.Single(x => x.Categoria == "Bebidas").CantidadVentas);
    }

    [Fact]
    public void T38_IgnoraVentasPendientes()
    {
        var p = Producto(1, "Arroz", 20, 2);
        var pendiente = new Venta(1, DateTime.Today, new Cliente("Prueba", "321"),
            new MedioPago(1, TipoMedioPago.Efectivo, "Efectivo"));
        pendiente.AgregarDetalle(new DetalleVenta(p, 2, p.obtenerPrecio()));
        Assert.Empty(ConsultasColecciones.ContabilizarVentasPorCategoria([pendiente]));
    }

    [Fact]
    public void T39_OrdenaPorUnidadesVendidasYConservaSinVentas()
    {
        var arroz = Producto(1, "Arroz", 30, 2);
        var aceite = Producto(2, "Aceite", 30, 2);
        var agua = Producto(3, "Agua", 30, 2);
        var venta = VentaConfirmada(1, (arroz, 2), (aceite, 5));
        var resultado = ConsultasColecciones.OrdenarProductosPorVentas([arroz, agua, aceite], [venta]);
        Assert.Equal(new[] {"Aceite", "Arroz", "Agua"}, resultado.Select(x => x.Nombre));
        Assert.Equal(new[] {5, 2, 0}, resultado.Select(x => x.UnidadesVendidas));
    }

    [Fact]
    public void T39_EmpatesSeOrdenanPorNombre()
    {
        var z = Producto(1, "Zanahoria", 10, 1);
        var a = Producto(2, "Arroz", 10, 1);
        var resultado = ConsultasColecciones.OrdenarProductosPorVentas([z, a], []);
        Assert.Equal(new[] {"Arroz", "Zanahoria"}, resultado.Select(x => x.Nombre));
    }
}
