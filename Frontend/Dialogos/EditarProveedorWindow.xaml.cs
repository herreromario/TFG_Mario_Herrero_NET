using StockMeal.Backend.Modelos;
using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    public partial class EditarProveedorWindow : Window
    {
        public Proveedor ProveedorEditado { get; }

        public EditarProveedorWindow(Proveedor proveedorOriginal)
        {
            InitializeComponent();
            ProveedorEditado = new Proveedor
            {
                IdProveedor = proveedorOriginal.IdProveedor,
                Nombre = proveedorOriginal.Nombre,
                Telefono = proveedorOriginal.Telefono,
                Direccion = proveedorOriginal.Direccion
            };
            DataContext = ProveedorEditado;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProveedorEditado.Nombre))
            {
                Mensajes.Error(this, "El nombre es obligatorio.");
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
