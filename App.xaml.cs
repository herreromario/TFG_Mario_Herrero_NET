using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StockMeal.Backend.Repositorios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using StockMeal.Backend.DBContext;
using StockMeal.Frontend.UserControls;
using StockMeal.Backend.MVVM.Propios;

namespace StockMeal
{
    public partial class App : Application
    {
        // Inyección de dependencias
        private IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        public App()
        {
            // Configuración de servicios para inyección de dependencias
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Construcción del proveedor de servicios
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Crear un scope de aplicación para mantener lifetimes scoped (DbContext, etc.)
            _serviceScope = _serviceProvider.CreateScope();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuración del DbContext
            services.AddDbContext<StockMealContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);

            // Configuración de logging
            services.AddLogging(configure => configure.AddConsole());

            // Repositorios genéricos
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Repositorios específicos
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            services.AddScoped<IProveedorRepository, ProveedorRepository>();
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IPedidoVentaRepository, PedidoVentaRepository>();
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            services.AddScoped<IMovimientoCajaRepository, MovimientoCajaRepository>();
            services.AddScoped<ICierreCajaRepository, CierreCajaRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IPermisoRepository, PermisoRepository>();
            services.AddScoped<IRecetaRepository, RecetaRepository>();
            services.AddScoped<IProduccionRepository, ProduccionRepository>();


            // -- SERVICIOS Y VISTAS --

            // Ventana principal
            services.AddScoped<MainWindow>();
            services.AddTransient<Login>();

            services.AddTransient<UCAñadirPlato>();
            services.AddTransient<UCListarPlatos>();
            services.AddTransient<UCEditarPlato>();
            services.AddTransient<UCAñadirIngrediente>();
            services.AddTransient<UCListarIngredientes>();
            services.AddTransient<UCEditarIngrediente>();
            services.AddTransient<UCAjustesStock>();
            services.AddTransient<UCListarProveedores>();
            services.AddTransient<UCAñadirProveedor>();
            services.AddTransient<UCEditarProveedor>();
            services.AddTransient<UCListarClientes>();
            services.AddTransient<UCAñadirCliente>();
            services.AddTransient<UCEditarCliente>();
            services.AddTransient<UCAñadirCompra>();
            services.AddTransient<UCListarCompras>();
            services.AddTransient<UCListarVentas>();
            services.AddTransient<UCListarFacturas>();
            services.AddTransient<UCDetalleCompra>();
            services.AddTransient<UCDetalleVenta>();
            services.AddTransient<UCListarMovimientosCaja>();
            services.AddTransient<UCListarCierresCaja>();
            services.AddTransient<UCListarEmpleados>();
            services.AddTransient<UCAñadirEmpleado>();
            services.AddTransient<UCEditarEmpleado>();
            services.AddTransient<UCListarRoles>();
            services.AddTransient<UCAñadirRol>();
            services.AddTransient<UCEditarRol>();
            services.AddTransient<UCListarPermisos>();
            services.AddTransient<UCAñadirPermiso>();
            services.AddTransient<UCEditarPermiso>();
            services.AddTransient<UCPerfilUsuario>();
            services.AddTransient<UCRegistrarProduccion>();
            services.AddTransient<UCHistorialProduccion>();
            services.AddTransient<UCRecetas>();
            services.AddTransient<UCListarRecetas>();
            services.AddTransient<UCAgregarReceta>();
            services.AddTransient<UCPlanificacionProduccion>();
            services.AddTransient<UCDashboard>();

            services.AddTransient<MVPlato>();
            services.AddTransient<MVIngrediente>();
            services.AddTransient<MVAjustesStock>();
            services.AddTransient<MVProveedor>();
            services.AddTransient<MVCliente>();
            services.AddTransient<MVCompra>();
            services.AddTransient<MVPedidoVenta>();
            services.AddTransient<MVFactura>();
            services.AddTransient<MVMovimientoCaja>();
            services.AddTransient<MVCierreCaja>();
            services.AddTransient<MVEmpleado>();
            services.AddTransient<MVRol>();
            services.AddTransient<MVPermiso>();
            services.AddTransient<MVReceta>();
            services.AddTransient<MVProduccion>();
            services.AddTransient<MVDashboard>();
            services.AddTransient<MVPerfilUsuario>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var ventanaLogin = _serviceScope.ServiceProvider.GetRequiredService<Login>();
            ventanaLogin.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceScope.Dispose();
            base.OnExit(e);
        }
    }
}
