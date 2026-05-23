using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCPerfilUsuario : UserControl
    {
        private readonly MVPerfilUsuario _mv;
        public UCPerfilUsuario(MVPerfilUsuario mv)
        {
            InitializeComponent();
            _mv = mv;
            DataContext = _mv;
        }

        private async void UCPerfilUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            _mv.passwordNueva = txtPasswordNueva.Password;
            var ok = await _mv.GuardarAsync();
            if (ok) Mensajes.Exito(Window.GetWindow(this), "Tu perfil se ha actualizado correctamente.");
            else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo actualizar el perfil.");
        }
    }
}
