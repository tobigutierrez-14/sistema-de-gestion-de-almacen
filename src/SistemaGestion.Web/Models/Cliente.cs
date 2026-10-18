using System;
using System.Collections.Generic;

namespace SistemaGestion.Web.Models;

public class Cliente
{
    // Contador estatico compartido por la clase
    private static int _siguienteId = 1;

    // Atributos privados
    private readonly int _id;
    private string _nombre;
    private string _dni;
    private string _telefono;
    private string _email;
    private readonly List<Venta> _historialCompras;

    // Constructor con validaciones y valores opcionales
    public Cliente(string nombre, string dni, string? telefono = null, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del cliente no puede estar vacío.", nameof(nombre));

        if (string.IsNullOrWhiteSpace(dni))
            throw new ArgumentException("El DNI del cliente no puede estar vacío.", nameof(dni));

        if (int.TryParse(dni.Trim(), out int dniNumerico) && dniNumerico <= 0)
            throw new ArgumentException("El DNI debe ser un número positivo.", nameof(dni));
            
        _id = _siguienteId++;
        _nombre = nombre.Trim();
        _dni = dni.Trim();
        _telefono = telefono ?? string.Empty;
        _email = email ?? string.Empty;
        _historialCompras = new List<Venta>();
    }
    // Sobrecarga para compatibilidad si el DNI se pasa como int
    public Cliente(string nombre, int dni, string? telefono = null, string? direccion = null)
        : this(nombre, dni <= 0 ? throw new ArgumentException("El DNI debe ser positivo.", nameof(dni)) : dni.ToString(), telefono, direccion){}

    // Getters
    public int getId() { return _id; }
    public string getNombre() { return _nombre; }
    public string getDni() { return _dni; }
    public string getTelefono() { return _telefono; }
    public string getEmail() { return _email; }
    public IReadOnlyList<Venta> getHistorialCompras() { return _historialCompras.AsReadOnly(); }

    // Setters
    public void setNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre del cliente no puede estar vacío.", nameof(nuevoNombre));

        _nombre = nuevoNombre.Trim();
    }
    public void setTelefono(string? nuevoTelefono) { _telefono = nuevoTelefono ?? string.Empty; }
    public void setEmail(string? nuevoEmail) { _email = nuevoEmail ?? string.Empty; }

    //Metodos

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