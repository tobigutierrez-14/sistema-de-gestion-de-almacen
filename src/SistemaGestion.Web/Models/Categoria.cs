namespace SistemaGestion.Web.Models
{
    public class Categoria
    {
        private int Id ;
        private string Nombre;
        private string Descripcion;

        public Categoria(int id, string nombre, string descripcion)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la categoría debe ser un número positivo.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
            }
            this.Id = id;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public void actualizarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
            }
            this.Nombre = nuevoNombre;
        }

        public void actualizarDescripcion(string nuevaDescripcion)
        {
            this.Descripcion = nuevaDescripcion;
        }

        public int obtenerId()
        {
            return this.Id;
        }

        public string obtenerNombre()
        {
            return this.Nombre;
        }

        public string obtenerDescripcion()
        {
            return this.Descripcion;
        }
    }

}
