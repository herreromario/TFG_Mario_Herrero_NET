using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using StockMeal.Backend.Seguridad;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVPerfilUsuario : MVBase
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        public Empleado? empleadoActual { get; private set; }
        public string usuario { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string passwordNueva { get; set; } = string.Empty;
        public string? error { get; set; }

        public MVPerfilUsuario(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task Inicializa()
        {
            if (string.IsNullOrWhiteSpace(SesionActual.Usuario))
                return;

            empleadoActual = await _empleadoRepository.GetByUsernameAsync(SesionActual.Usuario);
            if (empleadoActual == null) return;
            usuario = empleadoActual.Usuario;
            nombre = empleadoActual.Nombre;
            apellido = empleadoActual.Apellido;
            OnPropertyChanged(nameof(usuario));
            OnPropertyChanged(nameof(nombre));
            OnPropertyChanged(nameof(apellido));
        }

        public async Task<bool> GuardarAsync()
        {
            if (empleadoActual == null)
            {
                error = "No hay sesión activa.";
                return false;
            }

            var ok = await _empleadoRepository.ActualizarPerfilAsync(empleadoActual.IdEmpleado, usuario, nombre, apellido, passwordNueva);
            if (ok)
                SesionActual.Usuario = usuario;
            else
                error = "No se pudo actualizar el perfil. Revisa que el usuario no exista.";
            return ok;
        }
    }
}
