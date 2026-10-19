using System;

namespace SistemaGestion.Web.Models;

public abstract class Persona
{
    private static int _siguienteId = 1;

    // Atributos protegidos o privados accesibles vía métodos/getters
    private readonly int _id;
    private string _nombre;
    private string _telefono;
    private string _email;

    protected Persona(string nombre, string? telefono = null, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

        _id = _siguienteId++;
        _nombre = nombre.Trim();
        _telefono = telefono?.Trim() ?? string.Empty;
        _email = email?.Trim() ?? string.Empty;
    }

    // Getters comunes
    public int getId() {return _id;}
    public string getNombre() {return _nombre;}
    public string getTelefono() {return _telefono;}
    public string getEmail(){return _email;}

    // Setters / Modificadores comunes
    public void setNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));

        _nombre = nuevoNombre.Trim();
    }

    public void setTelefono(string? nuevoTelefono) => _telefono = nuevoTelefono?.Trim() ?? string.Empty;
    public void setEmail(string? nuevoEmail) => _email = nuevoEmail ?? string.Empty;
}