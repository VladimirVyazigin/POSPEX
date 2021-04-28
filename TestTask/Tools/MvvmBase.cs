using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace TestTask.Tools
{
    public class MvvmBase : LogableObject, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public MvvmBase()
        {
            _errors = new ConcurrentDictionary<string, List<string>>();
            ValidationLock = new object();
        }

        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (storage != null && storage.Equals(value)) return;
            storage = value;
            RaisePropertyChanged(propertyName);
        }

        public bool IsAutoAsyncValidationEnabled { get; set; }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (IsAutoAsyncValidationEnabled) ValidateAsync();
        }

        #endregion

        #region INotifyDataErrorInfo members

        private readonly ConcurrentDictionary<string, List<string>> _errors;
        protected readonly object ValidationLock;

        protected void RaiseErrorsChanged(string propertyName)
        {
            var ec = ErrorsChanged;
            ec?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors()
        {
            lock (ValidationLock)
            {
                _errors.Clear();
                RaiseHasErrorsChanged();
            }
        }

        protected void RaiseHasErrorsChanged()
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(nameof(HasErrors)));
        }

        public void ValidateAsync()
        {
            new TaskFactory().StartNew(Validate).ContinueWith(task =>
            {
                Application.Current?.Dispatcher.InvokeAsync(RaiseHasErrorsChanged);
            });
        }

        public void Validate()
        {
            lock (ValidationLock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in _errors.ToList())
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        List<string> outLi;
                        _errors.TryRemove(kv.Key, out outLi);
                        RaiseErrorsChanged(kv.Key);
                    }

                var q = from r in validationResults
                    from m in r.MemberNames
                    group r by m
                    into g
                    select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (_errors.ContainsKey(prop.Key))
                    {
                        List<string> outLi;
                        _errors.TryRemove(prop.Key, out outLi);
                    }

                    _errors.TryAdd(prop.Key, messages);
                    RaiseErrorsChanged(prop.Key);
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion
    }
}