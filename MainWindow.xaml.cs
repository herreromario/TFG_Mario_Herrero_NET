using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Backend.Repositorios;
using StockMeal.Frontend.Dialogos;
using StockMeal.Frontend.UserControls;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockMeal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Empleado _empleado;
        private IEmpleadoRepository _empleadoRepository;
        private ProductoRepository _productoRepository;

        private UserControl? _currentControl;

        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarDashboard();
        }

        private void Dashboard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MostrarDashboard();
        }

        private void MostrarDashboard()
        {
            var mv = _serviceProvider.GetRequiredService<MVDashboard>();
            MainContentHost.Content = new UCDashboard(mv);
        }

        private async void Platos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVPlato>();

            var listarPlatos = new UCListarPlatos(mv);

            await mv.Inicializa();

            listarPlatos.SolicitarAñadirPlato += () =>
            {
                MostrarAñadirPlato();
            };
            listarPlatos.SolicitarEditarPlato += MostrarEditarPlato;

            MainContentHost.Content = listarPlatos;
        }

        private void MostrarAñadirPlato()
        {
            var mv = _serviceProvider.GetRequiredService<MVPlato>();
            var añadirPlato = new UCAñadirPlato(mv);

            añadirPlato.SolicitarVolver += () =>
            {
                Platos_MouseDoubleClick(null!, null);
            };

            MainContentHost.Content = añadirPlato;
        }

        private async void Ingredientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVIngrediente>();

            var listaIngredientes = new UCListarIngredientes(mv);

            await mv.Inicializa();

            listaIngredientes.SolicitarAñadirIngrediente += () =>
            {
                MostrarAñadirIngrediente();
            };
            listaIngredientes.SolicitarEditarIngrediente += MostrarEditarIngrediente;

            MainContentHost.Content = listaIngredientes;
        }

        private async void AjustesStock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVAjustesStock>();
            var vista = new UCAjustesStock(mv);
            await mv.Inicializa();
            MainContentHost.Content = vista;
        }

        private void MostrarAñadirIngrediente()
        {
            var mv = _serviceProvider.GetRequiredService<MVIngrediente>();
            var añadirIngrediente = new UCAñadirIngrediente(mv);

            añadirIngrediente.SolicitarVolver += () =>
            {
                Ingredientes_MouseDoubleClick(null!, null);
            };

            MainContentHost.Content = añadirIngrediente;
        }

        private async void Proveedores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVProveedor>();
            var listarProveedores = new UCListarProveedores(mv);
            await mv.Inicializa();
            listarProveedores.SolicitarAñadirProveedor += MostrarAñadirProveedor;
            listarProveedores.SolicitarEditarProveedor += MostrarEditarProveedor;
            MainContentHost.Content = listarProveedores;
        }

        private void MostrarAñadirProveedor()
        {
            var mv = _serviceProvider.GetRequiredService<MVProveedor>();
            var añadirProveedor = new UCAñadirProveedor(mv);
            añadirProveedor.SolicitarVolver += () => Proveedores_MouseDoubleClick(null!, null!);
            MainContentHost.Content = añadirProveedor;
        }

        private async void Clientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVCliente>();
            var listarClientes = new UCListarClientes(mv);
            await mv.Inicializa();
            listarClientes.SolicitarAñadirCliente += MostrarAñadirCliente;
            listarClientes.SolicitarEditarCliente += MostrarEditarCliente;
            MainContentHost.Content = listarClientes;
        }

        private void MostrarEditarPlato(Producto plato)
        {
            var mv = _serviceProvider.GetRequiredService<MVPlato>();
            var vista = new UCEditarPlato(mv, plato);
            vista.SolicitarVolver += () => Platos_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarIngrediente(Producto ingrediente)
        {
            var mv = _serviceProvider.GetRequiredService<MVIngrediente>();
            var vista = new UCEditarIngrediente(mv, ingrediente);
            vista.SolicitarVolver += () => Ingredientes_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarProveedor(Proveedor proveedor)
        {
            var mv = _serviceProvider.GetRequiredService<MVProveedor>();
            var vista = new UCEditarProveedor(mv, proveedor);
            vista.SolicitarVolver += () => Proveedores_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarCliente(Cliente cliente)
        {
            var mv = _serviceProvider.GetRequiredService<MVCliente>();
            var vista = new UCEditarCliente(mv, cliente);
            vista.SolicitarVolver += () => Clientes_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarAñadirCliente()
        {
            var mv = _serviceProvider.GetRequiredService<MVCliente>();
            var añadirCliente = new UCAñadirCliente(mv);
            añadirCliente.SolicitarVolver += () => Clientes_MouseDoubleClick(null!, null!);
            MainContentHost.Content = añadirCliente;
        }

        private async void NuevaCompra_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVCompra>();
            var añadirCompra = new UCAñadirCompra(mv);
            await mv.Inicializa();
            añadirCompra.SolicitarVolver += () => HistorialCompras_MouseDoubleClick(null!, null!);
            MainContentHost.Content = añadirCompra;
        }

        private async void HistorialCompras_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVCompra>();
            var listarCompras = new UCListarCompras(mv);
            await mv.Inicializa();
            listarCompras.SolicitarVerDetalleCompra += MostrarDetalleCompra;
            MainContentHost.Content = listarCompras;
        }

        private async void HistorialVentas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVPedidoVenta>();
            var listarVentas = new UCListarVentas(mv);
            await mv.Inicializa();
            listarVentas.SolicitarVerDetalleVenta += MostrarDetalleVenta;
            MainContentHost.Content = listarVentas;
        }

        private void MostrarDetalleCompra(Compra compra)
        {
            var detalle = new UCDetalleCompra(compra);
            detalle.SolicitarVolver += () => HistorialCompras_MouseDoubleClick(null!, null!);
            MainContentHost.Content = detalle;
        }

        private void MostrarDetalleVenta(PedidoVentum venta)
        {
            var detalle = new UCDetalleVenta(venta);
            detalle.SolicitarVolver += () => HistorialVentas_MouseDoubleClick(null!, null!);
            MainContentHost.Content = detalle;
        }

        private async void Facturas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVFactura>();
            var listarFacturas = new UCListarFacturas(mv);
            await mv.Inicializa();
            MainContentHost.Content = listarFacturas;
        }

        private async void Recetas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVReceta>();
            var vista = new UCListarRecetas(mv);
            vista.SolicitarAgregarReceta += MostrarAgregarReceta;
            MainContentHost.Content = vista;
        }

        private void MostrarAgregarReceta()
        {
            var mv = _serviceProvider.GetRequiredService<MVReceta>();
            var vista = new UCAgregarReceta(mv);
            vista.SolicitarVolver += () => Recetas_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private async void MovimientosCaja_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVMovimientoCaja>();
            var vista = new UCListarMovimientosCaja(mv);
            await mv.Inicializa();
            MainContentHost.Content = vista;
        }

        private async void CierresCaja_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVCierreCaja>();
            var vista = new UCListarCierresCaja(mv);
            await mv.Inicializa();
            MainContentHost.Content = vista;
        }

        private async void Empleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVEmpleado>();
            var vista = new UCListarEmpleados(mv);
            await mv.Inicializa();
            vista.SolicitarAnadir += MostrarAnadirEmpleado;
            vista.SolicitarEditar += MostrarEditarEmpleado;
            MainContentHost.Content = vista;
        }

        private async void Roles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVRol>();
            var vista = new UCListarRoles(mv);
            await mv.Inicializa();
            vista.SolicitarAnadir += MostrarAnadirRol;
            vista.SolicitarEditar += MostrarEditarRol;
            MainContentHost.Content = vista;
        }

        private async void Permisos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVPermiso>();
            var vista = new UCListarPermisos(mv);
            await mv.Inicializa();
            vista.SolicitarAnadir += MostrarAnadirPermiso;
            vista.SolicitarEditar += MostrarEditarPermiso;
            MainContentHost.Content = vista;
        }

        private void MiCuenta_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mv = _serviceProvider.GetRequiredService<MVPerfilUsuario>();
            MainContentHost.Content = new UCPerfilUsuario(mv);
        }

        private void MostrarAnadirEmpleado()
        {
            var mv = _serviceProvider.GetRequiredService<MVEmpleado>();
            var vista = new UCAñadirEmpleado(mv);
            vista.SolicitarVolver += () => Empleados_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarEmpleado(Empleado e)
        {
            var mv = _serviceProvider.GetRequiredService<MVEmpleado>();
            var vista = new UCEditarEmpleado(mv, e);
            vista.SolicitarVolver += () => Empleados_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarAnadirRol()
        {
            var mv = _serviceProvider.GetRequiredService<MVRol>();
            var vista = new UCAñadirRol(mv);
            vista.SolicitarVolver += () => Roles_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarRol(Rol r)
        {
            var mv = _serviceProvider.GetRequiredService<MVRol>();
            var vista = new UCEditarRol(mv, r);
            vista.SolicitarVolver += () => Roles_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarAnadirPermiso()
        {
            var mv = _serviceProvider.GetRequiredService<MVPermiso>();
            var vista = new UCAñadirPermiso(mv);
            vista.SolicitarVolver += () => Permisos_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MostrarEditarPermiso(Permiso p)
        {
            var mv = _serviceProvider.GetRequiredService<MVPermiso>();
            var vista = new UCEditarPermiso(mv, p);
            vista.SolicitarVolver += () => Permisos_MouseDoubleClick(null!, null!);
            MainContentHost.Content = vista;
        }

        private void MenuExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is not Expander expanded)
                return;

            foreach (var child in Menu.Children)
            {
                if (child is Grid grid)
                {
                    foreach (var element in grid.Children)
                    {
                        if (element is Expander expander && !ReferenceEquals(expander, expanded))
                            expander.IsExpanded = false;
                    }
                }
            }
        }
    }
}
