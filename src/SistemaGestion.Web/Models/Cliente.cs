using System;
using System.Collections.Generic;

namespace SistemaGestion.Web.Models;

public class Cliente : Persona
{
    // Atributos específicos de Cliente (los de contacto se heredan de Persona)
    private readonly string _dni;
    private readonly List<Venta> _historialCompras;

    // Constructor principal con validaciones defensivas
    public Cliente(string nombre, string dni, string? telefono = null, string? email = null)
        : base(nombre, telefono, email)
    {
        if (string.IsNullOrWhiteSpace(dni))
            throw new ArgumentException("El DNI del cliente no puede estar vacío.", nameof(dni));

        string dniLimpio = dni.Trim();
        if (int.TryParse(dniLimpio, out int dniNumerico) && dniNumerico <= 0)
            throw new ArgumentException("El DNI debe ser un número positivo.", nameof(dni));

        if (dniLimpio == "0" || dniLimpio.StartsWith("-"))
            throw new ArgumentException("El DNI debe ser un número positivo.", nameof(dni));

        _dni = dniLimpio;
        _historialCompras = new List<Venta>();
    }

    // Sobrecarga para compatibilidad si el DNI se pasa como int
    public Cliente(string nombre, int dni, string? telefono = null, string? email = null)
        : this(nombre, dni <= 0 ? throw new ArgumentException("El DNI debe ser un número positivo.", nameof(dni)) : dni.ToString(), telefono, email)
    {
    }

    // Getter propio de Cliente (getId, getNombre, getTelefono y getEmail vienen de Persona)
    public string getDni() { return _dni; }
    public IReadOnlyList<Venta> getHistorialCompras() { return _historialCompras.AsReadOnly(); }

    // Métodos de negocio
    public void registrarCompra(Venta venta)
    {
        if (venta == null)
            throw new ArgumentNullException(nameof(venta), "La venta no puede ser nula.");

        _historialCompras.Add(venta);
    }

    public decimal calcularTotalCompras()
    {
        decimal total = 0;
        foreach (var venta in _historialCompras)
        {
            total += venta.CalcularTotal();
        }
        return total;
    }
}