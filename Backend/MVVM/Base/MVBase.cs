using MaterialDesignThemes.Wpf;
using StockMeal.Backend.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Backend.MVVM.Base
{
    public class MVBase : ValidatableViewModel
    {
        private int errorCount = 0;

        private bool _hasErrors;
        public bool HasErrors
        {
            get => _hasErrors;
            set => SetProperty(ref _hasErrors, value);
        }

        private void UpdateHasErrors()
        {
            HasErrors = errorCount == 0;

        }

        public SnackbarMessageQueue SnackbarMessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        public bool IsValid(DependencyObject obj)
        {
            // Revisa si el objeto tiene errores de validación
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }

        public void OnErrorEvent(object sender, RoutedEventArgs e)
        {
            var validationEventArgs = e as ValidationErrorEventArgs;
            if (validationEventArgs == null)
                throw new Exception("Argumentos inesperados");
            switch (validationEventArgs.Action)
            {
                case ValidationErrorEventAction.Added:
                    {
                        errorCount++; break;
                    }
                case ValidationErrorEventAction.Removed:
                    {
                        errorCount--; break;
                    }
                default:
                    {
                        throw new Exception("Acción desconocida");
                    }
            }
            UpdateHasErrors();
        }

        protected async Task<List<T>> GetAllAsync<T>(IGenericRepository<T> repo) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                result = new List<T>();
                throw new Exception("Database connection error occurred while fetching all elements.", ex);
            }
            return result.ToList();
        }

        protected async Task<T?> GetByIdAsync<T>(IGenericRepository<T> repo, int id) where T : class
        {
            try
            {
                return await repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al obtener elemento por ID: {ex.Message}");
                return null;
            }
        }

        protected async Task<bool> AddAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            try
            {
                await repo.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al añadir elemento: {ex.Message}");
                return false;
            }
        }

        protected async Task<bool> UpdateAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            try
            {
                await repo.UpdateAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al actualizar elemento: {ex.Message}");
                return false;
            }
        }

        protected async Task<bool> DeleteAsync<T>(IGenericRepository<T> repo, int id) where T : class
        {
            try
            {
                await repo.RemoveByIdAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al eliminar elemento: {ex.Message}");
                return false;
            }
        }

        protected async Task<bool> AddOrUpdateAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            try
            {
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null)
                {
                    SnackbarMessageQueue.Enqueue("No se encontró la propiedad 'Id' en el tipo.");
                    return false;
                }

                var idValue = idProp.GetValue(entity);
                if (idValue == null || (int)idValue == 0)
                {
                    await repo.AddAsync(entity);
                    return true;
                }

                var existing = await repo.GetByIdAsync((int)idValue);
                if (existing == null)
                {
                    await repo.AddAsync(entity);
                }
                else
                {
                    await repo.UpdateAsync(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al añadir o actualizar elemento: {ex.Message}");
                return false;
            }
        }
    }
}
