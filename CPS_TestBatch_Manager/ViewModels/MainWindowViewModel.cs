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
        private Func<Models.Environment, IEnvironmentViewModel> _environmentViewModelCreator;
        private ITestCaseEditViewModel _selectedTestCaseViewModel;
        private IIOService _openFileDialogService;
        private IXmlSerializerService<EnvironmentSettings> _environmentSettingsDataProvider;

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

        public List<CPS_TestBatch_Manager.Models.Environment> Environments
        {
            get;
            private set;
        }

        private EnvironmentSettings _environmentSettings;

        public EnvironmentSettings EnvironmentSettings
        {
            get { return _environmentSettings; }
            private set
            {
                _environmentSettings = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IEnvironmentViewModel> EnvironmentViewModels { get; private set; }

        public MainWindowViewModel(Func<INavigationViewModel, EqTestCase, ITestCaseEditViewModel> testCaseEditVmCreator,
            IEventAggregator eventAggregator,
            Func<string, ITestCaseEditViewModel> testCaseEditVmCreator2,
            Func<Models.Environment, IEnvironmentViewModel> environmentViewModelCreator,
            INavigationViewModel navigationViewModel,
            IIOService openFileDialogService,
            IXmlSerializerService<EnvironmentSettings> environmentSettingsDataProvider)
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
                _eventAggregator.GetEvent<EnvironmentSelectedEvent>().Subscribe(OnEnvironmentSelected);
                _testCaseEditViewModelCreator2 = testCaseEditVmCreator2;
                _environmentViewModelCreator = environmentViewModelCreator;
                _testCaseEditViewModelCreator = testCaseEditVmCreator;
                _openFileDialogService = openFileDialogService;
                _environmentSettingsDataProvider = environmentSettingsDataProvider;

                TestCaseEditViewModels = new ObservableCollection<ITestCaseEditViewModel>();
                NavigationViewModel = navigationViewModel;

                CloseTabCommand = new RelayCommand(p => CloseTab(p));
                AddTestCaseCommand = new RelayCommand(p => AddTestCase(p));
                SelectEnvironmentCommand = new RelayCommand(p => SelectEnvironment(p));
                LoadTestSuiteCommand = new RelayCommand(p => LoadTestCaseSuiteFile());
                UncheckAllEnvironmentCommand = new RelayCommand(p => UncheckEnvironments());
                EnvironmentSettings = environmentSettingsDataProvider.XmlFileToObject();
                Environments = EnvironmentSettings.Environments;

                EnvironmentViewModels = new ObservableCollection<IEnvironmentViewModel>();

                foreach (var env in Environments)
                {
                    var envVM = environmentViewModelCreator(env);
                    EnvironmentViewModels.Add(envVM);
                }

            }
        }

        private void OnEnvironmentSelected(EnvironmentViewModel environmentVm)
        {
            foreach(var envVm in EnvironmentViewModels)
            {
                envVm.IsChecked = false;
            }

            environmentVm.IsChecked = true;
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

        public ICommand UncheckAllEnvironmentCommand { get; private set; }

        private void UncheckEnvironments()
        {
            //EnvironmentViewModels.Select(x => x.IsChecked = false);
        }

        #endregion
    }
}
