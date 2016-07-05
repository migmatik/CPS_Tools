using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Xml.Linq;
using CPS_TestBatch_Manager.DataProvider.Lookups;
using CPS_TestBatch_Manager.DataProvider;
using Prism.Events;
using CPS_TestBatch_Manager.Events;
using System.Windows.Controls;

namespace CPS_TestBatch_Manager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private Func<INavigationViewModel, EqTestCase, ITestCaseEditViewModel> _testCaseEditViewModelCreator;
        private Func<string, ITestCaseEditViewModel> _testCaseEditViewModelCreator2;
        private ITestCaseEditViewModel _selectedTestCaseViewModel;
        private IIOService _openFileDialogService;

        public INavigationViewModel NavigationViewModel { get; private set; }
        public ObservableCollection<ITestCaseEditViewModel> TestCaseEditViewModels { get; private set; }
        public string TestCaseSuiteFile { get; private set; }

        private bool _canAddTestCase = false;
        public bool CanAddTestCase
        {
            get { return _canAddTestCase; }
            set
            {
                _canAddTestCase = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<string> Environments
        {
            get;
            private set;
        }

        public MainWindowViewModel(Func<INavigationViewModel, EqTestCase, ITestCaseEditViewModel> testCaseEditVmCreator,
            IEventAggregator eventAggregator,
            Func<string, ITestCaseEditViewModel> testCaseEditVmCreator2,
            INavigationViewModel navigationViewModel,
            IIOService openFileDialogService)
        {
            if (IsInDesignMode)
            {
                TestCaseSuiteFile = "designTimeFile";
                //CurrentTestCaseSuite = TestCaseSuiteDataService.CreateTestCaseSuite(TestCaseSuiteFile);
                //TestCases = new ObservableCollection<TestBatch>(CurrentTestCaseSuite.TestBatches);
            }
            else
            {
                _eventAggregator = eventAggregator;
                _eventAggregator.GetEvent<TestCaseDeletedEvent>().Subscribe(OnTestCaseDeleted);
                _eventAggregator.GetEvent<TestCaseSavedEvent>().Subscribe(OnTestCaseSaved);
                _testCaseEditViewModelCreator2 = testCaseEditVmCreator2;

                _testCaseEditViewModelCreator = testCaseEditVmCreator;
                _openFileDialogService = openFileDialogService;

                TestCaseEditViewModels = new ObservableCollection<ITestCaseEditViewModel>();
                NavigationViewModel = navigationViewModel;

                CloseTabCommand = new RelayCommand(p => CloseTab(p));
                AddTestCaseCommand = new RelayCommand(p => AddTestCase(p));
                SelectEnvironmentCommand = new RelayCommand(p => SelectEnvironment(p));
                LoadTestSuiteCommand = new RelayCommand(p => LoadTestCaseSuiteFile());
                Environments = new ObservableCollection<string>() { "CAT", "CAS" };
            }
        }

        private void OnTestCaseSaved(EqTestCase testCase)
        {
            var navItemVm = NavigationViewModel.NavigationItems.SingleOrDefault(item => item.Id == testCase.Id);

            var newOrUpdatesVm = new NavigationItemViewModel(testCase.Id, testCase.Name);

            if (navItemVm != null)
            {
                NavigationViewModel.NavigationItems.Remove(navItemVm);
            }

            NavigationViewModel.NavigationItems.Add(newOrUpdatesVm);
            SelectedNavigationItemViewModel = newOrUpdatesVm;
        }

        private void OnTestCaseDeleted(int testCaseId)
        {
            var testCaseEditVmToClose = TestCaseEditViewModels.SingleOrDefault(vm => vm.TestCase.Id == testCaseId);

            if (testCaseEditVmToClose != null)
            {
                TestCaseEditViewModels.Remove(testCaseEditVmToClose);
            }
        }

        #region Binding Properties

        public ITestCaseEditViewModel SelectedTestCaseEditViewModel
        {
            get { return _selectedTestCaseViewModel; }
            set
            {
                _selectedTestCaseViewModel = value;
                RaisePropertyChanged();
            }
        }

        private NavigationItemViewModel _selectedNavigationItemViewModel;

        public NavigationItemViewModel SelectedNavigationItemViewModel
        {
            get { return _selectedNavigationItemViewModel; }
            set
            {
                _selectedNavigationItemViewModel = value;
                RaisePropertyChanged();

                if (_selectedNavigationItemViewModel != null)
                {
                    ShowDetails(_selectedNavigationItemViewModel.Id);
                }
            }
        }

        private void ShowDetails(int testCaseId)
        {
            var testCaseEditVm = TestCaseEditViewModels.SingleOrDefault(vm => vm.TestCase.Id == testCaseId);

            if (testCaseEditVm == null)
            {
                testCaseEditVm = _testCaseEditViewModelCreator2(TestCaseSuiteFile);
                TestCaseEditViewModels.Add(testCaseEditVm);
                testCaseEditVm.Load(testCaseId);
            }

            SelectedTestCaseEditViewModel = testCaseEditVm;
            SelecteTestCaseIndex = TestCaseEditViewModels.IndexOf(testCaseEditVm);
        }

        private int _selecteTestCaseIndex;

        public int SelecteTestCaseIndex
        {
            get { return _selecteTestCaseIndex; }
            set
            {
                _selecteTestCaseIndex = value;
                RaisePropertyChanged();
                var navItemVm = NavigationViewModel.NavigationItems.SingleOrDefault(x => x.Id == TestCaseEditViewModels[value].TestCase.Id);

                if (SelectedNavigationItemViewModel != navItemVm)
                {
                    SelectedNavigationItemViewModel = navItemVm;
                }
            }
        }

        #endregion

        #region Commands

        public ICommand CloseTabCommand { get; private set; }

        private void CloseTab(object obj)
        {
            var testCaseVm = (ITestCaseEditViewModel)obj;
            TestCaseEditViewModels.Remove(testCaseVm);

            if (TestCaseEditViewModels.Count == 0)
            {
                SelectedNavigationItemViewModel = null;
            }
        }

        public ICommand LoadTestSuiteCommand { get; private set; }

        private void LoadTestCaseSuiteFile()
        {
            string testSuiteFile = _openFileDialogService.OpenFileDialog();

            if (!string.IsNullOrEmpty(testSuiteFile) && (string.IsNullOrEmpty(TestCaseSuiteFile) || !TestCaseSuiteFile.Equals(testSuiteFile)))
            {
                TestCaseSuiteFile = testSuiteFile;
                TestCaseEditViewModels.Clear();
                NavigationViewModel.Load(testSuiteFile);
                CanAddTestCase = true;
            }
        }

        public ICommand AddTestCaseCommand { get; private set; }

        private void AddTestCase(object obj)
        {
            ITestCaseEditViewModel testCaseEditVm = _testCaseEditViewModelCreator2(TestCaseSuiteFile);
            TestCaseEditViewModels.Add(testCaseEditVm);
            testCaseEditVm.Load();
            SelectedTestCaseEditViewModel = testCaseEditVm;
        }

        public ICommand SelectEnvironmentCommand { get; private set; }

        private void SelectEnvironment(object obj)
        {
            var mi = obj as MenuItem;



            var parent = mi.Parent as MenuItem;

            foreach (MenuItem item in parent.Items)
            {
                item.IsChecked = false;
            }

            mi.IsChecked = true;

            // throw new NotImplementedException();
        }

        #endregion
    }
}
