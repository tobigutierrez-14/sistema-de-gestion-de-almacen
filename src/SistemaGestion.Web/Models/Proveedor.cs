using System;

namespace SistemaGestion.Web.Models;

public class Proveedor : Persona
{
    public Proveedor(string nombre, string? telefono = null, string? email = null)
        : base(nombre, telefono, email)
    {
    }
}