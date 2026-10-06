using System;

namespace SistemaGestion.Web;

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

    // Constructor con validaciones y valores opcionales
    public Cliente(string nombre, string dni, string? telefono, string? email)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del cliente no puede estar vacío.", nameof(nombre));

        if (string.IsNullOrWhiteSpace(dni))
            throw new ArgumentException("El DNI del cliente no puede estar vacío.", nameof(dni));

        _id = _siguienteId++;
        _nombre = nombre.Trim();
        _dni = dni.Trim();
        _telefono = telefono ?? string.Empty;
        _email = email ?? string.Empty;
    }

    // Getters
    public int getId() { return _id; }
    public string getNombre() { return _nombre; }
    public string getDni() { return _dni; }
    public string getTelefono() { return _telefono; }
    public string getEmail() { return _email; }

    // Setters
    public void setTelefono(string? nuevoTelefono) { _telefono = nuevoTelefono ?? string.Empty; }
    public void setEmail(string? nuevoEmail) { _email = nuevoEmail ?? string.Empty; }
}