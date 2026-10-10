using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests
{
    public class CategoriaTests
    {
        [Fact]
        public void Constructor_DatosValidos_CreaCategoriaCorrectamente()
        {
            // Arrange
            var categoria = new Categoria(1, "Almacén", "Productos generales de almacén");

            // Assert
            Assert.Equal(1, categoria.obtenerId());
            Assert.Equal("Almacén", categoria.obtenerNombre());
            Assert.Equal("Productos generales de almacén", categoria.obtenerDescripcion()
            );
        }

        [Fact]
        public void ActualizarNombre_NombreValido_ModificaNombre()
        {
            // Arrange
            var categoria = new Categoria(1,"Almacén", "Productos generales");

            // Act
            categoria.actualizarNombre("Dietética");

            // Assert
            Assert.Equal("Dietética", categoria.obtenerNombre());
        }

        [Fact]
        public void Constructor_NombreVacio_LanzaExcepcion()
        {
            // Act + Assert
            Assert.Throws<ArgumentException>(() => new Categoria(1," ", "Productos generales"));
        }
    }
}
//caso normal: Constructor_DatosValidos_CreaCategoriaCorrectamente() --> Comprueba que La categoría se crea con los datos correctos.
//caso borde: ActualizarNombre_NombreValido_ModificaNombre() --> Comprueba que el nombre se modifica correctamente.
//caso error: Constructor_NombreVacio_LanzaExcepcion() --> Comprueba que Se lanza ArgumentException si el nombre está vacío.
