using System;
namespace SistemaGestion.Web;

public class Proveedor
{
    //Contador estatico compartido por la clase
    private static int _siguienteId = 1;

    //Atributos
    private readonly int _id;
    private string _nombre;
    private string _telefono;
    private string _email;

    //Constructor
    public Proveedor (string nombre, string? telefono, string? email)
    {
        if (string.IsNullOrWhiteSpace(nombre))
         throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(nombre));
        
        _id = _siguienteId++;
        _nombre = nombre.Trim();                  //.Trim() Elimina espacios en blanco que sobran al principio y al final del texto. 
        _telefono = telefono ?? string.Empty;     //Si se ingresa un null, lo cambia por un string vacio ("").
        _email = email ?? string.Empty;
    }

    //Getters
    public int getId(){return _id;}
    public string getNombre(){return _nombre;}
    public string getTelefono(){return _telefono;}
    public string getEmail(){return _email;}

    //Setters
    public void setTelefono(string? nuevoTelefono){_telefono = nuevoTelefono ?? string.Empty;}
    public void setEmail(string? nuevoEmail){_email = nuevoEmail ?? string.Empty;}
}