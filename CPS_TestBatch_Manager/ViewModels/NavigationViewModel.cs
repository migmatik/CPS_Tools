using CPS_TestBatch_Manager.DataProvider.Lookups;
using CPS_TestBatch_Manager.Events;
using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.Wrappers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.ViewModels
{
    public interface INavigationViewModel
    {
        void Load(string filename);
        ObservableCollection<EqTestCase> TestCases { get; set; }
        ObservableCollection<NavigationItemViewModel> NavigationItems { get; set; }
        EqTestSuite TestSuite { get; }
    }

    public class NavigationViewModel : INavigationViewModel
    {
        //private readonly ILookupProvider<EqTestCase> _eqTestCaseLookUpProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<string, ILookupProvider<EqTestCase>> _eqTestCaseLookUpCreator;

        public ObservableCollection<NavigationItemViewModel> NavigationItems { get; set; }
        public ObservableCollection<EqTestCase> TestCases { get; set; }
        public EqTestSuite TestSuite { get; private set; }

        public NavigationViewModel(IEventAggregator eventAggregator, Func<string, ILookupProvider<EqTestCase>> eqTestCaseLookUpProvider)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<TestCaseDeletedEvent>().Subscribe(OnTestCaseDeletedEvent);
            //_eventAggregator.GetEvent<TestCaseSavedEvent>().Subscribe(OnTestCaseSavedEvent);
            _eqTestCaseLookUpCreator = eqTestCaseLookUpProvider;
            NavigationItems = new ObservableCollection<NavigationItemViewModel>();
            TestCases = new ObservableCollection<EqTestCase>();
        }

        //private void OnTestCaseSavedEvent(EqTestCase savedTestCase)
        //{
        //    var navItem = NavigationItems.SingleOrDefault(item => item.Id == savedTestCase.Id);

        //    if(navItem != null)
        //    {
        //        navItem.DisplayValue = savedTestCase.Name;
        //    }
        //    else
        //    {
        //        //load testsuite file...
        //    }
        //}

        private void OnTestCaseDeletedEvent(int testCaseId)
        {
            var navItemToRemove = NavigationItems.SingleOrDefault(i => i.Id == testCaseId);

            if (navItemToRemove != null)
            {
                NavigationItems.Remove(navItemToRemove);
            }
        }

        public void Load(string filename)
        {
            if (NavigationItems != null)
            {
                NavigationItems.Clear();
            }

            var lookupService = _eqTestCaseLookUpCreator(filename);

            foreach (var testCaselookupItem in lookupService.GetLookup())
            {
                NavigationItems.Add(new NavigationItemViewModel(testCaselookupItem.Id, testCaselookupItem.DisplayValue));
            }

        }
    }

    public class NavigationItemViewModel : ViewModelBase, IValidatableTrackingObject
    {
        private int _id;
        private string _displayValue;

        public NavigationItemViewModel(int id, string displayValue)
        {
            _id = id;
            _displayValue = displayValue;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return DisplayValue;
        }


        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }

        public void RejectChanges()
        {
            throw new NotImplementedException();
        }

        public void AcceptChanges()
        {
            throw new NotImplementedException();
        }

        public bool IsChanged
        {
            get { throw new NotImplementedException(); }
        }
    }
}
