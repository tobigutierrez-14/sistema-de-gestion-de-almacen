namespace SistemaGestion.Web.Models
{
    public class Producto
    {
        private int Id;
        private string Nombre;
        private string Descripcion;
        private decimal Precio;
        private int Stock;
        private int StockMinimo;
        private Categoria Categoria;

        public Producto(int id, string nombre, string descripcion, decimal precio, int stock, int stockMinimo, Categoria categoria)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del producto debe ser un número positivo.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            }

            if (precio < 0)
            {
                throw new ArgumentException("El precio del producto no puede ser negativo.");
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock del producto no puede ser negativo.");
            }

            this.Id = id;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Precio = precio;
            this.Stock = stock;
            this.StockMinimo = stockMinimo;
            this.Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria), "La categoría no puede ser nula.");
        }
        public void AumentarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException(
                    "La cantidad debe ser mayor a cero.");

            Stock += cantidad;
        }
        public void DisminuirStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException(
                    "La cantidad debe ser mayor a cero.");

            if (cantidad > Stock)
                throw new InvalidOperationException(
                    "No hay stock suficiente.");

            Stock -= cantidad;
        }
        public bool HayStockDisponible(int cantidad)
        {
            if (cantidad <= 0)
                return false;

            return Stock >= cantidad;
        }
        public bool NecesitaReposicion()
        {
            return Stock <= StockMinimo;
        }
        public void actualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio < 0)
            {
                throw new ArgumentException("El precio del producto no puede ser negativo.");
            }
            this.Precio = nuevoPrecio;
        }        
        public void actualizarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            }
            this.Nombre = nuevoNombre;
        }
        
        public int obtenerStock()
        {
            return this.Stock;
        }
        public decimal obtenerPrecio()
        {
            return this.Precio;
        }

        public string obtenerNombre()
        {
            return this.Nombre;
        }
        public Categoria obtenerCategoria()
        {
            return this.Categoria;
        }
    }
}