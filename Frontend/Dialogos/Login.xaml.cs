using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;
using StockMeal.Backend.Repositorios;
using StockMeal.Backend.Seguridad;
using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly MainWindow _ventanaPrincipal;

        public Login(IEmpleadoRepository empleadoRepository,
                     MainWindow ventanaPrincipal)
        {
            InitializeComponent();
            _ventanaPrincipal = ventanaPrincipal;
            _empleadoRepository = empleadoRepository;
        }

        private async void btn_Entrar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) || !string.IsNullOrEmpty(txtPassword.Password))
            {
                bool isAuthenticated = await _empleadoRepository.LoginAsync(txtUsuario.Text, txtPassword.Password);
                if (isAuthenticated)
                {
                    SesionActual.Usuario = txtUsuario.Text;
                    _ventanaPrincipal.Show();
                    this.Close();
                }
                else
                {
                    Mensajes.Error(this, "Usuario o contraseña incorrectos.");
                    return;
                }
            }
            else
            {
                Mensajes.Error(this, "Por favor, introduzca usuario y clave.");
            }
        }
    }
}
