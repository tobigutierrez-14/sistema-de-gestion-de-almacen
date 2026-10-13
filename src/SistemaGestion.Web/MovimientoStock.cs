using System;

namespace SistemaGestion.Web.Models;

public class MovimientoStock
{
    private static int _siguienteId = 1;

    private readonly int _id;
    private readonly string _fecha;
    private readonly string _tipo;
    private readonly int _cantidad;
    private readonly string _motivo;
    private readonly Producto _producto;

    public MovimientoStock(Producto producto, string tipo, int cantidad, string motivo, string? fecha = null)
    {
        if (producto == null)
            throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");

        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("El tipo de movimiento no puede estar vacío.", nameof(tipo));

        string tipoNormalizado = tipo.Trim().ToUpper();
        if (tipoNormalizado != "INGRESO" && tipoNormalizado != "EGRESO")
            throw new ArgumentException("El tipo de movimiento debe ser 'INGRESO' o 'EGRESO'.", nameof(tipo));

        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo no puede estar vacío.", nameof(motivo));

        _id = _siguienteId++;
        _producto = producto;
        _tipo = tipoNormalizado;
        _cantidad = cantidad;
        _motivo = motivo.Trim();
        _fecha = string.IsNullOrWhiteSpace(fecha) ? DateTime.Now.ToString("yyyy-MM-dd") : fecha.Trim();
    }

    public int getId() => _id;
    public string getFecha() => _fecha;
    public string getTipo() => _tipo;
    public int getCantidad() => _cantidad;
    public string getMotivo() => _motivo;
    public Producto getProducto() => _producto;
}