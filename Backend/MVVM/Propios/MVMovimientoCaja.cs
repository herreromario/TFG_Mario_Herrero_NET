using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVMovimientoCaja : MVBase
    {
        private readonly IMovimientoCajaRepository _movimientoCajaRepository;
        public ListCollectionView listaMovimientos { get; private set; }

        public MVMovimientoCaja(IMovimientoCajaRepository movimientoCajaRepository)
        {
            _movimientoCajaRepository = movimientoCajaRepository;
        }

        public async Task Inicializa()
        {
            var movimientos = await _movimientoCajaRepository.GetMovimientosConRelacionesAsync();
            listaMovimientos = new ListCollectionView(movimientos);
            OnPropertyChanged(nameof(listaMovimientos));
        }
    }
}
