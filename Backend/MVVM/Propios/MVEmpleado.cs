using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Modelos;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVEmpleado : MVBase
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IRolRepository _rolRepository;
        public ListCollectionView listaEmpleados { get; private set; }
        public List<Rol> listaRoles { get; private set; } = new();
        public Empleado empleado { get; set; } = new();
        public string? error { get; set; }

        public MVEmpleado(IEmpleadoRepository empleadoRepository, IRolRepository rolRepository)
        {
            _empleadoRepository = empleadoRepository;
            _rolRepository = rolRepository;
        }

        public async Task Inicializa()
        {
            var empleados = await _empleadoRepository.GetEmpleadosConRolAsync();
            listaEmpleados = new ListCollectionView(empleados);
            listaRoles = await _rolRepository.GetAllAsync();
            OnPropertyChanged(nameof(listaEmpleados));
            OnPropertyChanged(nameof(listaRoles));
        }

        public async Task<bool> GuardarNuevoAsync()
        {
            try
            {
                await _empleadoRepository.AddAsync(empleado);
                await Inicializa();
                return true;
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> ActualizarAsync(Empleado e)
        {
            try
            {
                await _empleadoRepository.UpdateAsync(e);
                await Inicializa();
                return true;
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public async Task<bool> EliminarAsync(Empleado e)
        {
            try
            {
                await _empleadoRepository.RemoveAsync(e);
                await Inicializa();
                return true;
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }
    }
}
