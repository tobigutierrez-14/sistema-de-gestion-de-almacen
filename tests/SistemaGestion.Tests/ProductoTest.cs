using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests
{
    //Caso base
    public class ProductoTests
    {
        [Fact]
        public void AumentarStock_StockInicial10_Aumentar5_StockFinal15()
        {
            // Arrange
            var categoria = new Categoria(1, "Categoria1", "descripcion de categoria");
            var producto = new Producto(1, "Producto1", "descripcion de producto", 100.00m, 10, 5, categoria);

            // Act
            producto.AumentarStock(5);

            // Assert
            Assert.Equal(15, producto.obtenerStock());
        }
        //Caso borde
        [Fact]
        public void disminuirStock_StockInicial10_Disminuir10_StockFinal0()
        {
            // Arrange
            var categoria = new Categoria(1, "Categoria1", "descripcion de categoria");
            var producto = new Producto(1, "Producto1", "descripcion de producto", 100.00m, 10, 5, categoria);

            // Act
            producto.DisminuirStock(10);

            // Assert
            Assert.Equal(0, producto.obtenerStock());
        }
        //caso error
        [Fact]
        public void DisminuirStock_StockInicial10_Disminuir15_LanzaExcepcion()
        {
            // Arrange
            var categoria = new Categoria(1, "Categoria1", "descripcion de categoria");
            var producto = new Producto(1, "Producto1", "descripcion de producto", 100.0m, 10, 5, categoria);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => producto.DisminuirStock(15));
            Assert.Equal("No hay stock suficiente.", exception.Message);
        }
    }
    
}
// caso normal: AumentarStock_StockInicial10_Aumentar5_StockFinal15() --> comprueba que el stock aumenta correctamente
// caso borde: disminuirStock_StockInicial10_Disminuir10_StockFinal0() --> comprueba que se pueda consumir todo el stock
// caso error: DisminuirStock_StockInicial10_Disminuir15_LanzaExcepcion() --> comprueba que se lanza una excepción al intentar disminuir más stock del disponible