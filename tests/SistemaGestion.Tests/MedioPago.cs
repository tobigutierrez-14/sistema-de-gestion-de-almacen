using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests
{
    public class MedioPagoTests
    {
        [Fact]
        public void Constructor_DatosValidos_CreaMedioPagoCorrectamente()
        {
            // Arrange
            var medioPago = new MedioPago(1, TipoMedioPago.Efectivo, "Pago en efectivo");

            // Assert
            Assert.Equal(1, medioPago.obtenerId());
            Assert.Equal(TipoMedioPago.Efectivo, medioPago.obtenerTipo());
            Assert.Equal("Pago en efectivo", medioPago.obtenerDescripcion());
        }

        [Fact]
        public void ActualizarDescripcion_DescripcionValida_ModificaDescripcion()
        {
            // Arrange
            var medioPago = new MedioPago(1, TipoMedioPago.Efectivo, "Pago en efectivo");

            // Act
            medioPago.actualizarDescripcion("Pago en caja");

            // Assert
            Assert.Equal("Pago en caja", medioPago.obtenerDescripcion());
        }

        [Fact]
        public void Constructor_IdNegativo_LanzaExcepcion()
        {
            // Act + Assert
            Assert.Throws<ArgumentException>(() => new MedioPago(0, TipoMedioPago.Efectivo, "Pago en efectivo"));
        }
    }
}
//caso normal: Comprueba que se crea el medio de pago con sus datos correctos. 
//caso borde: Comprueba que la descripción se actualiza correctamente.
//caso error: Comprueba que Se lanza ArgumentException cuando el ID es cero o negativo.