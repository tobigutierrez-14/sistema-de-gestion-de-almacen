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
    private readonly List<MovimientoStock> _movimientos = [];
    private int _siguienteId = 1;
    private int _siguienteCategoriaId = 5;

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

    public IReadOnlyList<CategoriaFilaViewModel> ListarCategorias()
    {
        lock (_sync)
            return _categorias.Select(c => new CategoriaFilaViewModel(
                c.obtenerId(), c.obtenerNombre(), c.obtenerDescripcion() ?? "",
                _productos.Count(p => p.Producto.obtenerCategoria().obtenerId() == c.obtenerId())))
                .OrderBy(c => c.Nombre).ToArray();
    }

    public CategoriaFormViewModel? ObtenerCategoria(int id)
    {
        lock (_sync)
        {
            var categoria = _categorias.FirstOrDefault(c => c.obtenerId() == id);
            return categoria is null ? null : new CategoriaFormViewModel
            {
                Id = categoria.obtenerId(), Nombre = categoria.obtenerNombre(),
                Descripcion = categoria.obtenerDescripcion() ?? ""
            };
        }
    }

    public bool NombreCategoriaOcupado(string nombre, int? idExcluir = null)
    {
        lock (_sync)
            return _categorias.Any(c => c.obtenerId() != idExcluir &&
                string.Equals(c.obtenerNombre().Trim(), nombre.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public void AgregarCategoria(CategoriaFormViewModel datos)
    {
        lock (_sync)
        {
            ValidarCategoria(datos.Nombre, datos.Descripcion);
            if (NombreCategoriaOcupado(datos.Nombre))
                throw new ArgumentException("Ya existe una categoría con ese nombre.");
            var categoria = new Categoria(_siguienteCategoriaId, datos.Nombre.Trim(), datos.Descripcion.Trim());
            _categorias.Add(categoria);
            _siguienteCategoriaId++;
        }
    }

    public bool ActualizarCategoria(int id, CategoriaFormViewModel datos)
    {
        lock (_sync)
        {
            var categoria = _categorias.FirstOrDefault(c => c.obtenerId() == id);
            if (categoria is null) return false;
            ValidarCategoria(datos.Nombre, datos.Descripcion);
            if (NombreCategoriaOcupado(datos.Nombre, id))
                throw new ArgumentException("Ya existe una categoría con ese nombre.");
            // Los productos mantienen la referencia a esta misma instancia.
            categoria.actualizarNombre(datos.Nombre.Trim());
            categoria.actualizarDescripcion(datos.Descripcion.Trim());
            return true;
        }
    }

    private static void ValidarCategoria(string nombre, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 100 ||
            (descripcion?.Length ?? 0) > 400)
            throw new ArgumentException("Revisá el nombre y la descripción de la categoría.");
    }

    public StockIndexViewModel ResumenStock(string? busqueda, int? categoriaId, string? estado, string? tipo)
    {
        lock (_sync)
        {
            var productos = _productos.Select(Fila).ToArray();
            var filtrados = productos.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(busqueda))
                filtrados = filtrados.Where(p => p.Nombre.Contains(busqueda.Trim(), StringComparison.OrdinalIgnoreCase));
            if (categoriaId.HasValue)
            {
                var categoria = _categorias.FirstOrDefault(c => c.obtenerId() == categoriaId.Value)?.obtenerNombre();
                filtrados = filtrados.Where(p => categoria is not null && p.Categoria == categoria);
            }
            if (estado == "bajo") filtrados = filtrados.Where(p => p.StockBajo);
            if (estado == "normal") filtrados = filtrados.Where(p => !p.StockBajo);
            var movimientos = _movimientos.AsEnumerable().Reverse();
            if (tipo is "INGRESO" or "EGRESO") movimientos = movimientos.Where(m => m.getTipo() == tipo);
            return new StockIndexViewModel
            {
                Busqueda = busqueda ?? "", CategoriaId = categoriaId, Estado = estado ?? "todos", Tipo = tipo ?? "todos",
                Categorias = Categorias(), Productos = filtrados.ToArray(),
                TotalProductos = productos.Length, ProductosBajoStock = productos.Count(x => x.StockBajo),
                TotalUnidades = productos.Sum(x => (long)x.Stock),
                Movimientos = movimientos.Select(m => new MovimientoStockFilaViewModel(
                    m.getId(), m.getProducto().obtenerNombre(), m.getTipo(), m.getCantidad(),
                    m.getMotivo(), m.getFecha())).ToArray()
            };
        }
    }

    public void RegistrarMovimiento(StockMovimientoFormViewModel datos)
    {
        lock (_sync)
        {
            var producto = _productos.FirstOrDefault(p => p.Id == datos.ProductoId).Producto;
            if (producto is null) throw new ArgumentException("Seleccioná un producto válido.");
            if (datos.Tipo is not ("INGRESO" or "EGRESO")) throw new ArgumentException("Seleccioná un tipo válido.");
            if (datos.Cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(datos.Motivo)) throw new ArgumentException("El motivo es obligatorio.");
            if (datos.Tipo == "EGRESO" && !producto.HayStockDisponible(datos.Cantidad))
                throw new InvalidOperationException("No hay stock suficiente para registrar el egreso.");

            // Polimorfismo: cada subclase aplica la operación que le corresponde.
            MovimientoStock movimiento = datos.Tipo == "INGRESO"
                ? new IngresoStock(producto, datos.Cantidad, datos.Motivo)
                : new EgresoStock(producto, datos.Cantidad, datos.Motivo);

            // Si Aplicar() falla, no se agrega el movimiento al historial.
            movimiento.Aplicar();
            _movimientos.Add(movimiento);
        }
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
                    p.Producto.obtenerNombre(), p.Producto.obtenerDescripcion(), p.Producto.obtenerStock())).ToArray(),
                Actividades = _movimientos.AsEnumerable().Reverse().Take(5)
                    .Select(m => new ActividadReciente(
                        $"{(m.getTipo() == "INGRESO" ? "Ingreso" : "Egreso")} · {m.getProducto().obtenerNombre()}",
                        $"{m.getCantidad()} unidades · {m.getMotivo()}", m.getFecha(), "stock"))
                    .ToArray()
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
            if (diferencia != 0)
            {
                var cantidad = Math.Abs(diferencia);
                MovimientoStock movimiento = diferencia > 0
                    ? new IngresoStock(actual, cantidad, "Ajuste desde edición de producto")
                    : new EgresoStock(actual, cantidad, "Ajuste desde edición de producto");

                movimiento.Aplicar();
                _movimientos.Add(movimiento);
            }
            _productos[index] = (id, actual, actual.obtenerDescripcion(), actual.obtenerStockMinimo());
            return true;
        }
    }
}
