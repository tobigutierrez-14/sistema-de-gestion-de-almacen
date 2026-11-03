using SistemaGestion.Web.Models;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Services;

// Almacenamiento temporal sin base de datos.
public sealed class ClientesEnMemoria
{
    private readonly object _sync = new();
    private readonly List<Cliente> _clientes = [];

    public IReadOnlyList<(int Id, string Nombre, string Dni)> OpcionesVentas()
    {
        lock (_sync) return _clientes.OrderBy(c => c.getNombre())
            .Select(c => (c.getId(), c.getNombre(), c.getDni())).ToArray();
    }

    public Cliente? ObtenerInstancia(int id)
    {
        lock (_sync) return _clientes.FirstOrDefault(c => c.getId() == id);
    }

    public void RegistrarCompra(Cliente cliente, Venta venta)
    {
        lock (_sync)
        {
            if (!_clientes.Any(c => ReferenceEquals(c, cliente)))
                throw new ArgumentException("Seleccioná un cliente válido.");
            cliente.registrarCompra(venta);
        }
    }

    public IReadOnlyList<ClienteFilaViewModel> Listar(string? busqueda)
    {
        lock (_sync)
        {
            var consulta = _clientes.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var filtro = busqueda.Trim();
                consulta = consulta.Where(c =>
                    c.getNombre().Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.getDni().Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.getEmail().Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.getTelefono().Contains(filtro, StringComparison.OrdinalIgnoreCase));
            }
            return consulta.OrderBy(c => c.getNombre()).Select(c => new ClienteFilaViewModel(
                c.getId(), c.getNombre(), c.getDni(), c.getTelefono(), c.getEmail(),
                c.getHistorialCompras().Count, c.calcularTotalCompras())).ToArray();
        }
    }

    public ClienteFormViewModel? Obtener(int id)
    {
        lock (_sync)
        {
            var cliente = _clientes.FirstOrDefault(c => c.getId() == id);
            return cliente is null ? null : new ClienteFormViewModel
            {
                Id = cliente.getId(), Nombre = cliente.getNombre(), Dni = cliente.getDni(),
                Telefono = cliente.getTelefono(), Email = cliente.getEmail()
            };
        }
    }

    public bool DniOcupado(string dni, int? idExcluir = null)
    {
        lock (_sync)
            return _clientes.Any(c => c.getId() != idExcluir &&
                string.Equals(c.getDni(), dni.Trim(), StringComparison.Ordinal));
    }

    public void Agregar(ClienteFormViewModel datos)
    {
        lock (_sync)
        {
            if (DniOcupado(datos.Dni))
                throw new ArgumentException("Ya existe un cliente registrado con ese DNI.");
            var cliente = new Cliente(datos.Nombre, datos.Dni, datos.Telefono, datos.Email);
            _clientes.Add(cliente);
        }
    }

    public bool Actualizar(int id, ClienteFormViewModel datos)
    {
        lock (_sync)
        {
            var cliente = _clientes.FirstOrDefault(c => c.getId() == id);
            if (cliente is null) return false;
            if (!string.Equals(cliente.getDni(), datos.Dni, StringComparison.Ordinal))
                throw new ArgumentException("El DNI no puede modificarse desde esta pantalla.");
            // La instancia se conserva para no perder su historial de compras.
            cliente.setNombre(datos.Nombre);
            cliente.setTelefono(datos.Telefono);
            cliente.setEmail(datos.Email?.Trim() ?? string.Empty);
            return true;
        }
    }
}
