namespace SistemaGestion.Web.Models
{
    public class MedioPago
    {
        private int Id;
        private TipoMedioPago Tipo;
        private string Descripcion;

        public MedioPago(int id, TipoMedioPago tipo, string descripcion)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del tipo de medio de pago debe ser un número positivo.");
            }
            
            this.Id = id;
            this.Tipo = tipo;
            this.Descripcion = descripcion;
        }
        public void actualizarDescripcion(string nuevaDescripcion)
        {
            this.Descripcion = nuevaDescripcion;
        }

        public int obtenerId()
        {
            return this.Id;
        }

        public TipoMedioPago obtenerTipo()
        {
            return this.Tipo;
        }

        public string obtenerDescripcion()
        {
            return this.Descripcion;
        }
    }
}