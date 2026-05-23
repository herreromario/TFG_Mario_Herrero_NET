using StockMeal.Backend.Modelos;
using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    public partial class EditarClienteWindow : Window
    {
        public Cliente ClienteEditado { get; }

        public EditarClienteWindow(Cliente clienteOriginal)
        {
            InitializeComponent();
            ClienteEditado = new Cliente
            {
                IdCliente = clienteOriginal.IdCliente,
                Nombre = clienteOriginal.Nombre,
                Apellido = clienteOriginal.Apellido,
                Telefono = clienteOriginal.Telefono,
                TipoCliente = clienteOriginal.TipoCliente
            };
            DataContext = ClienteEditado;

            TipoClienteCombo.ItemsSource = new[] { "habitual", "nuevo", "grupo" };
            TipoClienteCombo.SelectedItem = ClienteEditado.TipoCliente;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClienteEditado.Nombre))
            {
                Mensajes.Error(this, "El nombre es obligatorio.");
                return;
            }

            ClienteEditado.TipoCliente = TipoClienteCombo.SelectedItem?.ToString() ?? "nuevo";
            DialogResult = true;
            Close();
        }
    }
}
