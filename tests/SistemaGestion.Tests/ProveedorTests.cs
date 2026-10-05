using System;
using Xunit;                    //Permite usar las herramientas de pruebas.
using SistemaGestion.Web;       

namespace SistemaGestion.Tests;

public class ProveedorTests
{
    [Fact]
    public void CrearProveedor_DatosValidos_GuardaInformacionCorrecta()
    {
        // 1. Arrange: se preparan los datos a cargar.
        string nombreEsperado = "Distribuidora Azul";
        string telefonoEsperado = "11223344";
        string emailEsperado = "azuldistro@gmail.com";
        
        // 2. Act: se crea el proveedor con datos bien cargados.
        var proveedor = new Proveedor(nombreEsperado, telefonoEsperado, emailEsperado);

        // 3. Assert: se verifica que todos los getters devuelvan exactamente lo que ingresamos.
        Assert.Equal(nombreEsperado, proveedor.getNombre());
        Assert.Equal(telefonoEsperado, proveedor.getTelefono());
        Assert.Equal(emailEsperado, proveedor.getEmail());
        Assert.True(proveedor.getId() > 0);    
    }

    [Fact]
    public void CrearProveedor_ContactoNull_GuardaStringVacio()
    {
        // 1. Arrange: se preparan los datos a cargar.
        string nombreEsperado = "Distribuidora sin contacto";
        
        // 2. Act: se crea el proveedor con datos bien cargados.
        var proveedor = new Proveedor(nombreEsperado, null, null);

        // 3. Assert: se verifica que todos los getters devuelvan exactamente lo que ingresamos.
        Assert.Equal(string.Empty, proveedor.getTelefono());
        Assert.Equal(string.Empty, proveedor.getEmail());  
    }

    [Fact]
    public void CrearProveedor_NombreVacio_LanzaExcepcion()
    {
        // 1. Arrange: se preparan los datos a cargar.
        string nombreInvalido = ""; // Nombre vacío

        // 2. Act & Assert: se espera que al crear el proveedor con un nombre inválido, se lance una excepción.
        Assert.Throws<ArgumentException>(() => new Proveedor(nombreInvalido, "12345678", "test@gmail.com"));
    }
}