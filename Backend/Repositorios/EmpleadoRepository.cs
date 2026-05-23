using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
    {
        private readonly ILogger<GenericRepository<Empleado>> _logger;

        public EmpleadoRepository(StockMealContext context, ILogger<GenericRepository<Empleado>> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Intenta autenticar un empleado por nombre y contraseña.
        /// 
        /// Devuelve la entidad <see cref="Empleado"/> si las credenciales son correctas, o null en caso contrario.
        /// 
        /// Nota: el método compara la cadena de contraseña tal cual. Si usas hashing (recomendado), aplica el
        /// verificador de hash aquí antes de comparar.
        /// 
        /// <param name="username">Nombre de usuario del empleado.</param>
        /// <param name="password">Contraseña en texto plano. </param>
        /// <returns> Empleado autenticado o null si las credenciales no son válidas.</returns>
        /// 
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            bool isAuthenticated = false;

            try
            {
                var usuario = await Query(asNoTracking: true)
                    .FirstOrDefaultAsync(e => e.Usuario == username)
                    .ConfigureAwait(false);

                if (usuario != null && usuario.Password == password)
                {
                    isAuthenticated = true;
                }
                return isAuthenticated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar el empleado {Username}.", username);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(newPassword));

            try
            {
                // Obtener el empleado por ID
                var empleado = await GetByIdAsync(userId).ConfigureAwait(false);
                if (empleado == null)
                {
                    _logger.LogWarning("Cambio de contraseña: empleado con id {Id} no encontrado.", userId);
                    return false;
                }

                // Verificar la contraseña actual
                if (empleado.Password != currentPassword)
                {
                    _logger.LogWarning("Cambio de contraseña fallido: contraseña actual incorrecta para el empleado con id {Id}.", userId);
                    return false;
                }

                // Actualizar la contraseña
                empleado.Password = newPassword;

                // Guardar los cambios
                await UpdateAsync(empleado).ConfigureAwait(false);

                _logger.LogInformation("Contraseña actualizada correctamente para el empleado con id {Id}.", userId);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar la contraseña del empleado con id {Id}.", userId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un empleado por su nombre de usuario.
        /// </summary>
        /// <param name="username">Nombre de usuario del empleado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Empleado o null si no existe.</returns>
        public async Task<Empleado?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                .FirstOrDefaultAsync(e => e.Usuario == username, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Comprueba si un usuario existe por username.
        /// </summary>
        /// <param name="username">Nombre de usuario a comprobar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si existe, false en caso contrario.</returns>
        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                .AnyAsync(e => e.Usuario == username, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Empleado>> GetEmpleadosConRolAsync()
        {
            return await Query(asNoTracking: true, e => e.IdRolNavigation)
                .OrderBy(e => e.Nombre)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<bool> ActualizarPerfilAsync(int idEmpleado, string usuario, string? nombre, string? apellido, string? passwordNueva, CancellationToken cancellationToken = default)
        {
            var empleado = await GetByIdAsync(idEmpleado).ConfigureAwait(false);
            if (empleado == null) return false;

            var usuarioDuplicado = await Query(asNoTracking: true)
                .AnyAsync(e => e.Usuario == usuario && e.IdEmpleado != idEmpleado, cancellationToken)
                .ConfigureAwait(false);
            if (usuarioDuplicado) return false;

            empleado.Usuario = usuario;
            if (!string.IsNullOrWhiteSpace(nombre)) empleado.Nombre = nombre;
            if (!string.IsNullOrWhiteSpace(apellido)) empleado.Apellido = apellido;
            if (!string.IsNullOrWhiteSpace(passwordNueva)) empleado.Password = passwordNueva;

            await base.UpdateAsync(empleado).ConfigureAwait(false);
            return true;
        }
    }
}
