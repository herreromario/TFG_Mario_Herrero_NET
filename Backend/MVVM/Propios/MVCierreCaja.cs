using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVCierreCaja : MVBase
    {
        private readonly ICierreCajaRepository _cierreCajaRepository;
        public ListCollectionView listaCierres { get; private set; }

        public MVCierreCaja(ICierreCajaRepository cierreCajaRepository)
        {
            _cierreCajaRepository = cierreCajaRepository;
        }

        public async Task Inicializa()
        {
            var cierres = await _cierreCajaRepository.GetCierresAsync();
            listaCierres = new ListCollectionView(cierres);
            OnPropertyChanged(nameof(listaCierres));
        }
    }
}
