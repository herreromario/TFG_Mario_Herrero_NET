using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAñadirCliente : UserControl
    {
        private readonly MVCliente _mvCliente;
        public event Action? SolicitarVolver;

        public UCAñadirCliente(MVCliente mvCliente)
        {
            InitializeComponent();
            _mvCliente = mvCliente;
            DataContext = mvCliente;
        }

        private async void AñadirCliente_Loaded(object sender, RoutedEventArgs e) => await _mvCliente.Inicializa();

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvCliente.AnadirClienteAsync();
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Cliente añadido correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvCliente.error ?? "No se pudo guardar el cliente.");
            if (ok) SolicitarVolver?.Invoke();
        }
    }
}
