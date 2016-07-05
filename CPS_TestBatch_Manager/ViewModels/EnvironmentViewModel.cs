using CPS_TestBatch_Manager.Events;
using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Wrappers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CPS_TestBatch_Manager.ViewModels
{
    public interface IEnvironmentViewModel
    {
        EnvironmentWrapper Environment { get; set; }
        bool IsChecked { get; set; }
    }

    public class EnvironmentViewModel: ViewModelBase, IEnvironmentViewModel 
    {
        private readonly IEventAggregator _eventAggregator;

        public EnvironmentViewModel(IEventAggregator eventAggregator, Models.Environment environment)
        {
            _eventAggregator = eventAggregator;

            Environment = new EnvironmentWrapper(environment);                
        }

        private EnvironmentWrapper _environment;
        public EnvironmentWrapper Environment
        {
            get { return _environment; }
            set
            {
                _environment = value;
                RaisePropertyChanged();
            }
        }

        bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }

        private ICommand _checkCommand;
        public ICommand CheckCommand 
        { 
            get
            {
              if(_checkCommand == null)
              {
                  _checkCommand = new RelayCommand(p => Check(p));
              }

              return _checkCommand;
            }
            set
            {
                _checkCommand = value;
                RaisePropertyChanged();
            }


        }

        private void Check(object p)
        {
            IsChecked = true;
            _eventAggregator.GetEvent<EnvironmentSelectedEvent>().Publish(this);
        }

        public override string ToString()
        {
            return Environment.Name;
        }
    }
}
