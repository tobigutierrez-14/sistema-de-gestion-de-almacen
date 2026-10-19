using System;

namespace SistemaGestion.Web.Models;

public class EgresoStock : MovimientoStock
{
    public EgresoStock(Producto producto, int cantidad, string motivo, string? fecha = null)
        : base(producto, cantidad, motivo, fecha)
    {
    }

    public override string getTipo() => "EGRESO";

    public override void Aplicar()
    {
        getProducto().DisminuirStock(getCantidad());
    }
}