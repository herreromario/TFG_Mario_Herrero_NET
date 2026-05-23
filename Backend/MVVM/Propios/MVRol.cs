using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Modelos;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVRol : MVBase
    {
        private readonly IRolRepository _rolRepository;
        public ListCollectionView listaRoles { get; private set; }
        public Rol rol { get; set; } = new();
        public string? error { get; set; }

        public MVRol(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task Inicializa()
        {
            var roles = await _rolRepository.GetRolesConPermisosAsync();
            listaRoles = new ListCollectionView(roles);
            OnPropertyChanged(nameof(listaRoles));
        }

        public async Task<bool> GuardarNuevoAsync()
        {
            try { await _rolRepository.AddAsync(rol); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> ActualizarAsync(Rol r)
        {
            try { await _rolRepository.UpdateAsync(r); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> EliminarAsync(Rol r)
        {
            try { await _rolRepository.RemoveAsync(r); await Inicializa(); return true; }
            catch (Exception ex) { error = ex.Message; return false; }
        }
    }
}
