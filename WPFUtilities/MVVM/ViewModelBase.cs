using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities.MVVM
{
    using System.ComponentModel;
    using System.Reflection;

    public interface IViewModel : INotifyPropertyChanged { }

    public interface IViewModel<TModel> : IViewModel
    {
        [Browsable(false)]
        [Bindable(false)]
        TModel Model { get; set; }
    }

    [Serializable]
    public abstract class ViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event AsyncCompletedEventHandler InitializationCompleted;

        protected ViewModel()
        {
            var initializationTask = new Task(() => Initialize());
            initializationTask.ContinueWith(result => InitializationCompletedCallback(result));
            initializationTask.Start();
        }


        protected virtual void Initialize() { }

        private void InitializationCompletedCallback(IAsyncResult result)
        {
            var initializationCompleted = InitializationCompleted;
            if (initializationCompleted != null)
            {
                InitializationCompleted(this, new AsyncCompletedEventArgs(null, !result.IsCompleted, result.AsyncState));
            }
        }

        protected virtual void OnPropertyChanged(String propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public abstract class ViewModel<TModel> : ViewModel, IViewModel<TModel> where TModel : class
    {
        private TModel model;

        [Browsable(false)]
        [Bindable(false)]
        public TModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                if (Model != value)
                {
                    var properties = this.GetType().GetProperties(BindingFlags.Public);

                    var oldValues = properties.Select(p => p.GetValue(this, null));
                    var enumerator = oldValues.GetEnumerator();

                    model = value;

                    foreach (var property in properties)
                    {
                        enumerator.MoveNext();
                        var oldValue = enumerator.Current;
                        var newValue = property.GetValue(this, null);

                        if ((oldValue == null && newValue != null)
                        || (oldValue != null && newValue == null)
                        || (!oldValue.Equals(newValue)))
                        {
                            OnPropertyChanged(property.Name);
                        }
                    }
                }
            }
        }

        protected ViewModel(TModel model) 
            : base()
        {
            this.model = model;
        }

        public override int GetHashCode()
        {
            return Model.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as ViewModel<TModel>;

            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public bool Equals(IViewModel<TModel> other)
        {
            if (other == null)
                return false;

            if (Model == null)
                return Model == other.Model;

            return Model.Equals(other.Model);
        }
    }
}
