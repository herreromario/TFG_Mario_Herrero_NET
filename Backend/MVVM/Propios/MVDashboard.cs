using Microsoft.EntityFrameworkCore;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.MVVM.Base;
using System.Collections.ObjectModel;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVDashboard : MVBase
    {
        private readonly StockMealContext _context;

        public int TotalPlatos { get; private set; }
        public int TotalIngredientes { get; private set; }
        public int IngredientesStockBajo { get; private set; }
        public decimal ComprasHoy { get; private set; }
        public decimal VentasHoy { get; private set; }
        public decimal SaldoCajaHoy { get; private set; }

        public ObservableCollection<DashboardStockBajoItem> StockBajoItems { get; } = new();
        public ObservableCollection<DashboardMovimientoItem> UltimosMovimientos { get; } = new();

        public MVDashboard(StockMealContext context)
        {
            _context = context;
        }

        public async Task Inicializa()
        {
            var hoy = DateTime.Today;

            TotalPlatos = await _context.Productos.AsNoTracking().CountAsync(p => p.Tipo == "plato");
            TotalIngredientes = await _context.Productos.AsNoTracking().CountAsync(p => p.Tipo == "ingrediente");

            var ingredientes = await _context.Productos.AsNoTracking()
                .Where(p => p.Tipo == "ingrediente")
                .Select(p => new { p.Nombre, p.StockDisponible, p.StockMinimo })
                .ToListAsync();

            var stockBajo = ingredientes
                .Where(i => (i.StockDisponible ?? 0) <= (i.StockMinimo ?? 0))
                .OrderBy(i => (i.StockDisponible ?? 0) - (i.StockMinimo ?? 0))
                .Take(8)
                .ToList();

            IngredientesStockBajo = ingredientes.Count(i => (i.StockDisponible ?? 0) <= (i.StockMinimo ?? 0));

            StockBajoItems.Clear();
            foreach (var item in stockBajo)
            {
                StockBajoItems.Add(new DashboardStockBajoItem
                {
                    Nombre = item.Nombre,
                    StockDisponible = item.StockDisponible ?? 0,
                    StockMinimo = item.StockMinimo ?? 0
                });
            }

            var comprasHoy = await _context.Compras.AsNoTracking()
                .Where(c => c.Fecha.HasValue && c.Fecha.Value.Date == hoy)
                .ToListAsync();
            ComprasHoy = comprasHoy.Sum(c => c.ImporteTotal);

            var ventasHoy = await _context.PedidoVenta.AsNoTracking()
                .Where(v => v.Fecha.HasValue && v.Fecha.Value.Date == hoy && v.TipoTransaccion == "venta")
                .ToListAsync();
            VentasHoy = ventasHoy.Sum(v => v.ImporteTotal);

            var movimientosHoy = await _context.MovimientoCajas.AsNoTracking()
                .Where(m => m.Fecha.HasValue && m.Fecha.Value.Date == hoy)
                .ToListAsync();

            var ingresos = movimientosHoy.Where(m => m.TipoMovimiento == "ingreso").Sum(m => m.Importe);
            var gastos = movimientosHoy.Where(m => m.TipoMovimiento == "gasto").Sum(m => m.Importe);
            SaldoCajaHoy = ingresos - gastos;

            var ultimosMov = await _context.MovimientoCajas.AsNoTracking()
                .OrderByDescending(m => m.Fecha)
                .Take(8)
                .ToListAsync();

            UltimosMovimientos.Clear();
            foreach (var mov in ultimosMov)
            {
                UltimosMovimientos.Add(new DashboardMovimientoItem
                {
                    Fecha = mov.Fecha,
                    Tipo = mov.TipoMovimiento,
                    Importe = mov.Importe,
                    Descripcion = string.IsNullOrWhiteSpace(mov.Descripcion) ? "-" : mov.Descripcion
                });
            }

            OnPropertyChanged(nameof(TotalPlatos));
            OnPropertyChanged(nameof(TotalIngredientes));
            OnPropertyChanged(nameof(IngredientesStockBajo));
            OnPropertyChanged(nameof(ComprasHoy));
            OnPropertyChanged(nameof(VentasHoy));
            OnPropertyChanged(nameof(SaldoCajaHoy));
        }
    }

    public class DashboardStockBajoItem
    {
        public string Nombre { get; set; } = string.Empty;
        public int StockDisponible { get; set; }
        public int StockMinimo { get; set; }
    }

    public class DashboardMovimientoItem
    {
        public DateTime? Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
