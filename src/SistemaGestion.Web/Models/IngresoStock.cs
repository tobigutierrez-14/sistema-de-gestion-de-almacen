using System;

namespace SistemaGestion.Web.Models;

public class IngresoStock : MovimientoStock
{
    public IngresoStock(Producto producto, int cantidad, string motivo, string? fecha = null)
        : base(producto, cantidad, motivo, fecha)
    {
    }

    public override string getTipo() => "INGRESO";

    public override void Aplicar()
    {
        getProducto().AumentarStock(getCantidad());
    }
}