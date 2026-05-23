using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVPedidoVenta : MVBase
    {
        private readonly IPedidoVentaRepository _pedidoVentaRepository;
        public ListCollectionView listaVentas { get; private set; }
        public string? error { get; set; }

        public MVPedidoVenta(IPedidoVentaRepository pedidoVentaRepository)
        {
            _pedidoVentaRepository = pedidoVentaRepository;
        }

        public async Task Inicializa()
        {
            var ventas = await _pedidoVentaRepository.GetPedidosConRelacionesAsync();
            listaVentas = new ListCollectionView(ventas);
            OnPropertyChanged(nameof(listaVentas));
        }
    }
}
