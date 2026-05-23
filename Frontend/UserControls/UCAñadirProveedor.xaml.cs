using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAñadirProveedor : UserControl
    {
        private readonly MVProveedor _mvProveedor;
        public event Action? SolicitarVolver;

        public UCAñadirProveedor(MVProveedor mvProveedor)
        {
            InitializeComponent();
            _mvProveedor = mvProveedor;
            DataContext = mvProveedor;
        }

        private async void AñadirProveedor_Loaded(object sender, RoutedEventArgs e) => await _mvProveedor.Inicializa();

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvProveedor.AnadirProveedorAsync();
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Proveedor añadido correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvProveedor.error ?? "No se pudo guardar el proveedor.");
            if (ok) SolicitarVolver?.Invoke();
        }
    }
}
