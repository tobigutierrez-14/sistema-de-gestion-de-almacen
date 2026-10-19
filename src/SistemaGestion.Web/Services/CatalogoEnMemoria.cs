using SistemaGestion.Web.Models;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Services;

/// <summary>Demo sin persistencia: los cambios desaparecen al reiniciar el servidor.</summary>
public sealed class CatalogoEnMemoria
{
    private readonly object _sync = new();
    private readonly List<Categoria> _categorias =
    [
        new(1, "Almacén", "Productos secos y envasados"),
        new(2, "Bebidas", "Bebidas"),
        new(3, "Lácteos", "Lácteos y refrigerados"),
        new(4, "Limpieza", "Artículos de limpieza")
    ];
    private readonly List<(int Id, Producto Producto, string Descripcion, int StockMinimo)> _productos = [];
    private int _siguienteId = 1;

    public CatalogoEnMemoria()
    {
        Agregar(new() { Nombre = "Arroz largo fino 1 kg", Descripcion = "Paquete de arroz", Precio = 1950m, Stock = 38, StockMinimo = 10, CategoriaId = 1 });
        Agregar(new() { Nombre = "Aceite de girasol 900 ml", Descripcion = "Botella 900 ml", Precio = 3400m, Stock = 7, StockMinimo = 10, CategoriaId = 1 });
        Agregar(new() { Nombre = "Agua mineral 1.5 L", Descripcion = "Agua sin gas", Precio = 1250m, Stock = 24, StockMinimo = 8, CategoriaId = 2 });
        Agregar(new() { Nombre = "Leche entera 1 L", Descripcion = "Leche larga vida", Precio = 1700m, Stock = 4, StockMinimo = 12, CategoriaId = 3 });
        Agregar(new() { Nombre = "Detergente 750 ml", Descripcion = "Lavavajillas", Precio = 2350m, Stock = 19, StockMinimo = 6, CategoriaId = 4 });
    }

    public IReadOnlyList<CategoriaOpcionViewModel> Categorias()
    {
        lock (_sync) return _categorias.Select(c => new CategoriaOpcionViewModel(c.obtenerId(), c.obtenerNombre())).ToArray();
    }

    public IReadOnlyList<ProductoFilaViewModel> Productos()
    {
        lock (_sync) return _productos.Select(p => Fila(p)).ToArray();
    }

    private static ProductoFilaViewModel Fila((int Id, Producto Producto, string Descripcion, int StockMinimo) p) =>
        new(p.Id, p.Producto.obtenerNombre(), p.Producto.obtenerDescripcion(), p.Producto.obtenerPrecio(),
            p.Producto.obtenerStock(), p.Producto.obtenerStockMinimo(),
            p.Producto.obtenerCategoria().obtenerNombre(), p.Producto.NecesitaReposicion());

    public DashboardViewModel ResumenDashboard()
    {
        lock (_sync)
        {
            var bajoStock = _productos.Where(p => p.Producto.NecesitaReposicion()).ToArray();
            return new DashboardViewModel
            {
                TotalProductos = _productos.Count,
                ProductosBajoStock = bajoStock.Length,
                Alertas = bajoStock.Select(p => new AlertaStock(
                    p.Producto.obtenerNombre(), p.Producto.obtenerDescripcion(), p.Producto.obtenerStock())).ToArray()
            };
        }
    }

    public ProductoFormViewModel? Obtener(int id)
    {
        lock (_sync)
        {
            var existente = _productos.FirstOrDefault(p => p.Id == id);
            if (existente.Producto is null) return null;
            return new()
            {
                Id = existente.Id,
                Nombre = existente.Producto.obtenerNombre(),
                Descripcion = existente.Descripcion,
                Precio = existente.Producto.obtenerPrecio(),
                Stock = existente.Producto.obtenerStock(),
                StockMinimo = existente.StockMinimo,
                CategoriaId = existente.Producto.obtenerCategoria().obtenerId()
            };
        }
    }

    public bool CategoriaExiste(int id)
    {
        lock (_sync) return _categorias.Any(c => c.obtenerId() == id);
    }

    public void Agregar(ProductoFormViewModel datos)
    {
        lock (_sync)
        {
            var categoria = _categorias.Single(c => c.obtenerId() == datos.CategoriaId);
            var id = _siguienteId++;
            var producto = new Producto(id, datos.Nombre.Trim(), datos.Descripcion.Trim(), datos.Precio, datos.Stock, datos.StockMinimo, categoria);
            _productos.Add((id, producto, datos.Descripcion.Trim(), datos.StockMinimo));
        }
    }

    public bool Actualizar(int id, ProductoFormViewModel datos)
    {
        lock (_sync)
        {
            var index = _productos.FindIndex(p => p.Id == id);
            if (index < 0) return false;
            var categoria = _categorias.Single(c => c.obtenerId() == datos.CategoriaId);
            var actual = _productos[index].Producto;
            // Actualizar el objeto existente para conservar su identidad y usar las reglas del dominio.
            // Validar primero para no dejar una edición parcialmente aplicada.
            if (string.IsNullOrWhiteSpace(datos.Nombre) || datos.Precio < 0 || datos.Stock < 0 ||
                datos.StockMinimo < 0)
                throw new ArgumentException("Los datos del producto no son válidos.");
            var diferencia = datos.Stock - actual.obtenerStock();
            actual.actualizarNombre(datos.Nombre.Trim());
            actual.actualizarPrecio(datos.Precio);
            actual.actualizarDescripcion(datos.Descripcion.Trim());
            actual.actualizarStockMinimo(datos.StockMinimo);
            actual.actualizarCategoria(categoria);
            if (diferencia > 0) actual.AumentarStock(diferencia);
            else if (diferencia < 0) actual.DisminuirStock(-diferencia);
            _productos[index] = (id, actual, actual.obtenerDescripcion(), actual.obtenerStockMinimo());
            return true;
        }
    }
}
