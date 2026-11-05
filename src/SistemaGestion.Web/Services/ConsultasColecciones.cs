using SistemaGestion.Web.Models;

namespace SistemaGestion.Web.Services;

/// <summary>
/// Consultas sobre colecciones del dominio para las tareas T36-T39.
/// No modifican productos, ventas ni existencias.
/// </summary>
public static class ConsultasColecciones
{
    public sealed record ResumenProducto(
        string Nombre, string Categoria, decimal Precio,
        int Stock, bool NecesitaReposicion);

    public sealed record VentasPorCategoria(string Categoria, int CantidadVentas);

    public sealed record ProductoPorVentas(string Nombre, int UnidadesVendidas);

    // Recorrido: filtrar los productos que cumplen una condición (Where).
    // La condición se apoya en la regla del dominio Producto.NecesitaReposicion().
    public static IReadOnlyList<Producto> FiltrarProductosConStockCritico(IEnumerable<Producto> productos)
    {
        ArgumentNullException.ThrowIfNull(productos);
        return productos.Where(p => p.NecesitaReposicion()).ToArray();
    }

    // Recorrido: filtrar los productos que NO cumplen la condición (Where + !).
    public static IReadOnlyList<Producto> FiltrarProductosSinStockCritico(IEnumerable<Producto> productos)
    {
        ArgumentNullException.ThrowIfNull(productos);
        return productos.Where(p => !p.NecesitaReposicion()).ToArray();
    }


    public static IReadOnlyList<ResumenProducto> TransformarProductos(IEnumerable<Producto> productos)
    {
        ArgumentNullException.ThrowIfNull(productos);
        return productos.Select(p => new ResumenProducto(
            p.obtenerNombre(), p.obtenerCategoria().obtenerNombre(),
            p.obtenerPrecio(), p.obtenerStock(), p.NecesitaReposicion()
        )).ToArray();
    }

    // T37 - Encontrar el primer producto que necesita reposición (FirstOrDefault).
    // Respeta el orden de la colección recibida y devuelve null si no existe.
    public static Producto? PrimerProductoConStockCritico(IEnumerable<Producto> productos)
    {
        ArgumentNullException.ThrowIfNull(productos);
        return productos.FirstOrDefault(p => p.NecesitaReposicion());
    }

    // T38 - Una venta confirmada se cuenta UNA vez por cada categoría
    // que aparezca en sus detalles, aunque incluya varios productos de esa categoría.
    public static IReadOnlyList<VentasPorCategoria> ContabilizarVentasPorCategoria(IEnumerable<Venta> ventas)
    {
        ArgumentNullException.ThrowIfNull(ventas);
        return ventas
            .Where(v => v.ObtenerEstado() == EstadoVenta.Completada)
            .SelectMany(v => v.ObtenerDetalles()
                .Select(d => d.getProducto().obtenerCategoria())
                .Select(c => (c.obtenerId(), c.obtenerNombre()))
                .DistinctBy(c => c.Item1))
            .GroupBy(c => c.Item1)
            .Select(g => new VentasPorCategoria(g.First().Item2, g.Count()))
            .OrderByDescending(x => x.CantidadVentas)
            .ThenBy(x => x.Categoria, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    // T39 - Ordena los productos por unidades vendidas (descendente).
    // Los productos sin ventas también aparecen, con cero unidades.
    public static IReadOnlyList<ProductoPorVentas> OrdenarProductosPorVentas(
        IEnumerable<Producto> productos, IEnumerable<Venta> ventas)
    {
        ArgumentNullException.ThrowIfNull(productos);
        ArgumentNullException.ThrowIfNull(ventas);
        var unidades = ventas
            .Where(v => v.ObtenerEstado() == EstadoVenta.Completada)
            .SelectMany(v => v.ObtenerDetalles())
            .GroupBy(d => d.getProducto())
            .ToDictionary(g => g.Key, g => g.Sum(d => d.getCantidad()));

        return productos
            .Select(p => new ProductoPorVentas(
                p.obtenerNombre(), unidades.GetValueOrDefault(p, 0)))
            .OrderByDescending(x => x.UnidadesVendidas)
            .ThenBy(x => x.Nombre, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
