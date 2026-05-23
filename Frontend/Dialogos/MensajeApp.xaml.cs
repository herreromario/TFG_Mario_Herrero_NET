using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    public partial class MensajeApp : Window
    {
        public MensajeApp(string titulo, string mensaje, bool confirmacion)
        {
            InitializeComponent();
            TituloText.Text = titulo;
            MensajeText.Text = mensaje;
            BtnCancelar.Visibility = confirmacion ? Visibility.Visible : Visibility.Collapsed;
            BtnAceptar.Content = confirmacion ? "Confirmar" : "Cerrar";
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
