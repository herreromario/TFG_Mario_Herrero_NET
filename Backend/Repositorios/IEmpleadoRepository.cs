using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IEmpleadoRepository
    {
        /// <summary>
        /// Intenta autenticar un empleado por nombre y contraseña.
        /// Devuelve la entidad <see cref="Empleado"/> si las credenciales son correctas, o null en caso contrario.
        /// </summary>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="password">Contraseña (texto plano o ya hasheada según la política de la app).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<bool> LoginAsync(string username, string password);


        /// <summary>
        /// Cambia la contraseña de un empleado verificando la contraseña actual.
        /// Devuelve true si la contraseña se actualizó correctamente, false si no se encontró el empleado o la contraseña actual no coincide.
        /// </summary>
        /// <param name="userId">ID del usuario.</param>
        /// <param name="currentPassword">Contraseña actual.</param>
        /// <param name="newPassword">Nueva contraseña.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un empleado por su nombre de usuario (sin tracking por defecto).
        /// </summary>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<Empleado?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);


        /// <summary>
        /// Comprueba si existe un empleado con el nombre proporcionado.
        /// </summary>
        /// <param name="username">Nombre de usuario a comprobar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<List<Empleado>> GetEmpleadosConRolAsync();
        Task AddAsync(Empleado empleado);
        Task UpdateAsync(Empleado empleado);
        Task RemoveAsync(Empleado empleado);
        Task<bool> ActualizarPerfilAsync(int idEmpleado, string usuario, string? nombre, string? apellido, string? passwordNueva, CancellationToken cancellationToken = default);
    }
}
