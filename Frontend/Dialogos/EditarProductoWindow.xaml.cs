using StockMeal.Backend.Modelos;
using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    public partial class EditarProductoWindow : Window
    {
        private readonly bool _esIngrediente;
        private readonly Producto _producto;

        public Producto ProductoEditado => _producto;

        public EditarProductoWindow(Producto productoOriginal, bool esIngrediente)
        {
            InitializeComponent();
            _esIngrediente = esIngrediente;
            _producto = new Producto
            {
                IdProducto = productoOriginal.IdProducto,
                Nombre = productoOriginal.Nombre,
                Descripcion = productoOriginal.Descripcion,
                Precio = productoOriginal.Precio,
                Tipo = productoOriginal.Tipo,
                Unidad = productoOriginal.Unidad,
                StockDisponible = productoOriginal.StockDisponible,
                StockMinimo = productoOriginal.StockMinimo
            };
            DataContext = _producto;

            if (_esIngrediente)
            {
                TituloText.Text = "Editar ingrediente";
                CampoExtraLabel.Text = "Unidad";
                CampoExtraText.Visibility = Visibility.Collapsed;
                UnidadCombo.Visibility = Visibility.Visible;
                UnidadCombo.ItemsSource = new[] { "unidad", "g", "ml", "lamina", "loncha" };
                UnidadCombo.SelectedItem = _producto.Unidad;
            }
            else
            {
                TituloText.Text = "Editar plato";
                CampoExtraLabel.Text = "Stock disponible";
                CampoExtraText.Text = _producto.StockDisponible?.ToString() ?? "0";
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_producto.Nombre))
            {
                Mensajes.Error(this, "El nombre es obligatorio.");
                return;
            }

            if (_esIngrediente)
            {
                _producto.Unidad = UnidadCombo.SelectedItem?.ToString() ?? "unidad";
            }
            else
            {
                if (!int.TryParse(CampoExtraText.Text, out var stock))
                {
                    Mensajes.Error(this, "Stock disponible inválido.");
                    return;
                }
                _producto.StockDisponible = stock;
            }

            DialogResult = true;
            Close();
        }
    }
}
