using System.Windows;

namespace StockMeal.Frontend.Dialogos
{
    public static class Mensajes
    {
        public static void Exito(Window? owner, string mensaje)
        {
            var w = new MensajeApp("Operación completada", mensaje, false) { Owner = owner };
            w.ShowDialog();
        }

        public static void Error(Window? owner, string mensaje)
        {
            var w = new MensajeApp("Ha ocurrido un error", mensaje, false) { Owner = owner };
            w.ShowDialog();
        }

        public static bool Confirmar(Window? owner, string mensaje)
        {
            var w = new MensajeApp("Confirmar acción", mensaje, true) { Owner = owner };
            return w.ShowDialog() == true;
        }
    }
}
