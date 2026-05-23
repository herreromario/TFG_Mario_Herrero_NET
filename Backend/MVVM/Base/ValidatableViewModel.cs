using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockMeal.Backend.MVVM.Base
{
    public abstract class ValidatableViewModel : BaseViewModel, IDataErrorInfo
    {
        public virtual string Error => null!;

        public virtual string this[string columnName]
        {
            get
            {
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateProperty(
                        GetType().GetProperty(columnName).GetValue(this)
                        , new ValidationContext(this)
                        {
                            MemberName = columnName
                        }
                        , validationResults))
                    return null;

                return validationResults.First().ErrorMessage;
            }
        }

        protected virtual string ValidateProperty(string propertyName) => string.Empty;

        protected void RaiseCommandsCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
