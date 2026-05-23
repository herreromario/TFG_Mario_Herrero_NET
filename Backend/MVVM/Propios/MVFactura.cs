using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVFactura : MVBase
    {
        private readonly IFacturaRepository _facturaRepository;
        public ListCollectionView listaFacturas { get; private set; }
        public string? error { get; set; }

        public MVFactura(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        public async Task Inicializa()
        {
            var facturas = await _facturaRepository.GetFacturasConPedidoAsync();
            listaFacturas = new ListCollectionView(facturas);
            OnPropertyChanged(nameof(listaFacturas));
        }
    }
}
