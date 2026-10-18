using System;
using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests;

public class ClienteTests
{
    [Fact]
    public void CrearCliente_DatosValidos_GuardaInformacionCorrecta()
    {
        // 1. Arrange: caso normal con datos válidos
        string nombreEsperado = "Juan Perez";
        string dniEsperado = "38111222";
        string telefonoEsperado = "11334455";
        string emailEsperado = "juan.perez@mail.com";

        // 2. Act: creamos la instancia
        var cliente = new Cliente(nombreEsperado, dniEsperado, telefonoEsperado, emailEsperado);

        // 3. Assert: verificamos que guarde los datos y genere un ID válido
        Assert.Equal(nombreEsperado, cliente.getNombre());
        Assert.Equal(dniEsperado, cliente.getDni());
        Assert.Equal(telefonoEsperado, cliente.getTelefono());
        Assert.Equal(emailEsperado, cliente.getEmail());
        Assert.True(cliente.getId() > 0);
        Assert.Empty(cliente.getHistorialCompras()); // Verificamos que el historial de compras esté vacío al inicio
    }

    [Fact]
    public void CrearCliente_ContactoNull_GuardaStringVacio()
    {
        // 1. Arrange: caso borde con teléfono y email nulos
        string nombreEsperado = "Maria Gomez";
        string dniEsperado = "40123456";

        // 2. Act: creamos el cliente con contactos en null
        var cliente = new Cliente(nombreEsperado, dniEsperado, null, null);

        // 3. Assert: verificamos que no rompa y guarde cadenas vacías
        Assert.Equal(string.Empty, cliente.getTelefono());
        Assert.Equal(string.Empty, cliente.getEmail());
    }

    [Fact]
    public void CrearCliente_DniVacio_LanzaExcepcion()
    {
        // 1. Arrange: caso de error con DNI vacío
        string dniInvalido = "";

        // 2 y 3. Act & Assert: verificamos que se defienda lanzando la excepción
        Assert.Throws<ArgumentException>(() => new Cliente("Carlos Lopez", dniInvalido, "11223344", "carlos@mail.com"));
    }

    [Fact]
    public void CrearCliente_ConDatosInvalidos_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Cliente("", "35123456"));
        Assert.Throws<ArgumentException>(() => new Cliente("   ", "35123456"));
        Assert.Throws<ArgumentException>(() => new Cliente("Carlos", "0"));
        Assert.Throws<ArgumentException>(() => new Cliente("Carlos", "-10"));
    }

    [Fact]
    public void RegistrarCompra_ConVentaValida_CalculaTotalComprado()
    {
        // Caso normal de interacción con Venta
        var cliente = new Cliente("Maria Lopez", 30123456);
        var categoria = new Categoria(1, "Bebidas", "Gaseosas");
        var producto = new Producto(1, "Agua 500ml", "Mineral", 100m, 50, 5, categoria);
        var medioPago = new MedioPago(1, TipoMedioPago.TarjetaCredito, "Visa");

        var venta = new Venta(1, DateTime.Now, cliente, medioPago);
        venta.AgregarDetalle(new DetalleVenta(producto, 2, 100m));

        cliente.registrarCompra(venta);

        Assert.Single(cliente.getHistorialCompras());
        Assert.Equal(200m, cliente.calcularTotalCompras());
    }

    [Fact]
    public void RegistrarCompra_ConVentaNula_LanzaArgumentNullException()
    {
        // Caso error de interacción
        var cliente = new Cliente("Maria Lopez", 30123456);

        Assert.Throws<ArgumentNullException>(() => cliente.registrarCompra(null!));
    }
}