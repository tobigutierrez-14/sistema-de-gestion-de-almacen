using System;

namespace SistemaGestion.Web.Models;

public abstract class MovimientoStock
{
    private static int _siguienteId = 1;

    private readonly int _id;
    private readonly string _fecha;
    private readonly int _cantidad;
    private readonly string _motivo;
    private readonly Producto _producto;

    protected MovimientoStock(Producto producto, int cantidad, string motivo, string? fecha = null)
    {
        if (producto == null)
            throw new ArgumentNullException(nameof(producto), "El producto asociado no puede ser nulo.");

        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo no puede estar vacío.", nameof(motivo));

        _id = _siguienteId++;
        _producto = producto;
        _cantidad = cantidad;
        _motivo = motivo.Trim();
        _fecha = string.IsNullOrWhiteSpace(fecha) ? DateTime.Now.ToString("yyyy-MM-dd") : fecha.Trim();
    }

    // Getters comunes
    public int getId() => _id;
    public string getFecha() => _fecha;
    public int getCantidad() => _cantidad;
    public string getMotivo() => _motivo;
    public Producto getProducto() => _producto;

    // Métodos polimórficos
    public abstract string getTipo();
    public abstract void Aplicar();
}