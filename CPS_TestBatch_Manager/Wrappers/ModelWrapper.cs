using CPS_TestBatch_Manager.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class ModelWrapper<T> : ViewModelBase, IValidatableTrackingObject, IValidatableObject 
    {
        private Dictionary<string, object> _originalValues;
        private List<IValidatableTrackingObject> _trackingObjects;

        public ModelWrapper(T model)
        {
            if (model == null) { throw new ArgumentNullException("model"); }

            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IValidatableTrackingObject>();
        }

        public T Model { get; private set; }

        public bool IsChanged 
        {
            get { return _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged); } 
        }

        public void AcceptChanges()
        {
            _originalValues.Clear();

            foreach(var trackingObject in _trackingObjects)
            {
                trackingObject.AcceptChanges();
            }

            RaisePropertyChanged("");
        }

        public void RejectChanges()
        {
            foreach(var origValue in _originalValues)
            {
                typeof(T).GetProperty(origValue.Key).SetValue(Model, origValue.Value);
            }

            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.RejectChanges();
            }

            RaisePropertyChanged("");
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            return (TValue)propertyInfo.GetValue(Model);           
        }

        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName) ? (TValue)_originalValues[propertyName] : GetValue<TValue>(propertyName);
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

        protected void SetValue<TValue>(TValue newValue, [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            var currentValue = propertyInfo.GetValue(Model);

            if (!Equals(currentValue, newValue))
            {
                UpdateOriginalValues(currentValue, newValue, propertyName);
                propertyInfo.SetValue(Model, newValue);
                RaisePropertyChanged(propertyName);
                RaisePropertyChanged(string.Concat(propertyName, "IsChanged"));                
            }
        }

        private void UpdateOriginalValues(object currentValue, object newValue, string propertyName)
        {
            if(!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                RaisePropertyChanged("IsChanged");
            }
            else
            {
                if(Equals(_originalValues[propertyName], newValue))
                {
                    _originalValues.Remove(propertyName);
                    RaisePropertyChanged("IsChanged");
                }
            }
        }

        protected void RegisterCollection<TWrapper, TModel>(
            ChangeTrackingCollection<TWrapper> wrapperCollection,
            List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                if( e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<TWrapper>())
                    {
                        modelCollection.Remove(item.Model);
                    }
                }

                if( e.NewItems != null)
                {
                    foreach(var item in e.NewItems.Cast<TWrapper>())
                    {
                        modelCollection.Add(item.Model);
                    }
                }
            };

            RegisterTrackingObject(wrapperCollection);
        }
        
        protected void RegisterComplexProperty<TModel>(ModelWrapper<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject<TTrackingObject>(TTrackingObject trackingObject)
            where TTrackingObject : IValidatableTrackingObject, INotifyPropertyChanged
        {
            if (!_trackingObjects.Contains(trackingObject))
            {
                _trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += wrapper_PropertyChanged;
            }
        }

        void wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsChanged")
            {
                RaisePropertyChanged("IsChanged");
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

        public bool IsValid
        {
            get { return _trackingObjects.All(t => t.IsValid); }
        }
    }
}
