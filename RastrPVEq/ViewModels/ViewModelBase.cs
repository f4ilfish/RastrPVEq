using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RastrPVEq.ViewModels
{
    /// <summary>
    /// ViewModelBase class
    /// </summary>
    internal class ViewModelBase : INotifyPropertyChanged, IDisposable, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// Property changed event field
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed method (Invoke / Raise)
        /// </summary>
        /// <param name="PropertyName">Property Name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Refresh property value method (cyclically)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Properties field reference</param>
        /// <param name="value">New properties field value</param>
        /// <param name="PropertyName">Property name</param>
        /// <returns>Is properties field value refresh</returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        #endregion

        #region IDisposable

        //~ViewModelBase()
        //{
        //    Dispose(false);
        //}

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _Disposed;

        /// <summary>
        /// Dispose method (releasing managed resources)
        /// </summary>
        /// <param name="Disposing"></param>
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
        }

        #endregion

        #region INotifyDataErrorInfo

        /// <summary>
        /// PropertyErrors field
        /// </summary>
        private readonly Dictionary<string, List<string>> _propertyErrors = new();

        /// <summary>
        /// HasErrors method
        /// </summary>
        public bool HasErrors => _propertyErrors.Any();

        /// <summary>
        /// HasErrorsByProperty method
        /// </summary>
        public bool HasErrorsByProperty(string propertyName) => _propertyErrors.ContainsKey(propertyName);

        /// <summary>
        /// ErrorsChanged event
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// Get propertie's errors method
        /// </summary>
        /// <param name="propertyName">Propety name</param>
        /// <returns></returns>
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        /// <summary>
        /// Add propertie's error method
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="errorMessage">Text message</param>
        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// OnErrorsChanged by property method
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Clear propertie's errors
        /// </summary>
        /// <param name="propertyName"></param>
        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        #endregion
    }
}
