using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Modelos;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVPermiso : MVBase
    {
        private readonly IPermisoRepository _permisoRepository;
        public ListCollectionView listaPermisos { get; private set; }
        public Permiso permiso { get; set; } = new();
        public string? error { get; set; }

        public MVPermiso(IPermisoRepository permisoRepository)
        {
            _permisoRepository = permisoRepository;
        }

        public async Task Inicializa()
        {
            var permisos = await _permisoRepository.GetPermisosAsync();
            listaPermisos = new ListCollectionView(permisos);
            OnPropertyChanged(nameof(listaPermisos));
        }

        public async Task<bool> GuardarNuevoAsync()
        {
            try { await _permisoRepository.AddAsync(permiso); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> ActualizarAsync(Permiso p)
        {
            try { await _permisoRepository.UpdateAsync(p); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> EliminarAsync(Permiso p)
        {
            try { await _permisoRepository.RemoveAsync(p); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }
    }
}
