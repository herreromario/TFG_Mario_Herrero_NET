using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVCliente : MVBase
    {
        private readonly IClienteRepository _clienteRepository;
        private List<Cliente> _listaClientes;
        private List<Predicate<Cliente>> _criterios;
        private Predicate<Cliente> _criterioNombre;
        private Predicate<object> _predicadoFiltros;

        public Cliente cliente { get; set; } = new Cliente { TipoCliente = "nuevo" };
        public List<string> listaTiposCliente { get; } = new() { "habitual", "nuevo", "grupo" };
        public ListCollectionView listaClientes { get; private set; }
        public string textoNombreCliente { get; set; } = string.Empty;
        public string? error { get; set; }

        public MVCliente(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task Inicializa()
        {
            _listaClientes = await _clienteRepository.GetClientesAsync();
            listaClientes = new ListCollectionView(_listaClientes);
            _criterios = new List<Predicate<Cliente>>();
            _criterioNombre = c => !string.IsNullOrWhiteSpace(textoNombreCliente)
                && c.Nombre.ToLower().StartsWith(textoNombreCliente.ToLower());
            _predicadoFiltros = new Predicate<object>(FiltroCriterios);
            OnPropertyChanged(nameof(listaClientes));
        }

        public void Filtrar()
        {
            _criterios.Clear();
            if (!string.IsNullOrWhiteSpace(textoNombreCliente))
                _criterios.Add(_criterioNombre);
            listaClientes.Filter = _predicadoFiltros;
        }

        private bool FiltroCriterios(object item) => _criterios.TrueForAll(x => x((Cliente)item));

        public async Task<bool> AnadirClienteAsync()
        {
            try
            {
                await _clienteRepository.AddAsync(cliente);
                cliente = new Cliente { TipoCliente = "nuevo" };
                OnPropertyChanged(nameof(cliente));
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> ActualizarClienteAsync(Cliente clienteEditado)
        {
            try
            {
                await _clienteRepository.UpdateAsync(clienteEditado);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> EliminarClienteAsync(Cliente clienteAEliminar)
        {
            try
            {
                await _clienteRepository.RemoveAsync(clienteAEliminar);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }
    }
}
