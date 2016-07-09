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
using CPS_TestBatch_Manager.Views.Dialogs;
using System.Diagnostics;
using CPS_TestBatch_Manager.Configuration;
using System.Configuration;

namespace CPS_TestBatch_Manager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private Func<string, IEnvironment, ITestCaseEditViewModel> _testCaseEditViewModelCreator;      
        private Func<IEnvironment, IEnvironmentViewModel> _environmentViewModelCreator;
        private ITestCaseEditViewModel _selectedTestCaseViewModel;
        private IIOService _openFileDialogService;       
        private readonly IMessageDialogService _messageDialogService;

        public string ApplicationVersion { get; set; }
        public INavigationViewModel NavigationViewModel { get; private set; }
        public ObservableCollection<ITestCaseEditViewModel> TestCaseEditViewModels { get; private set; }

        private EnvironmentSettings _environmentConfig;

        public EnvironmentSettings EnvironmentConfig
        {
            get { return _environmentConfig; }
            set
            {
                _environmentConfig = value;
                RaisePropertyChanged();
            }
        }


        private string _testCaseSuiteFile;
        public string TestCaseSuiteFile
        {
            get { return _testCaseSuiteFile; }
            private set
            {
                _testCaseSuiteFile = value;
                RaisePropertyChanged();
            }
        }

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

        private IEnvironmentViewModel _selectedEnvironmentViewModel;

        public IEnvironmentViewModel SelectedEnvironmentViewModel
        {
            get { return _selectedEnvironmentViewModel; }
            set
            {
                _selectedEnvironmentViewModel = value;
                RaisePropertyChanged();
            }
        }
        
        public ObservableCollection<IEnvironmentViewModel> EnvironmentViewModels { get; private set; }

        public MainWindowViewModel(
            IEventAggregator eventAggregator,
            Func<string, IEnvironment, ITestCaseEditViewModel> testCaseEditVmCreator,           
            Func<IEnvironment, IEnvironmentViewModel> environmentViewModelCreator,
            INavigationViewModel navigationViewModel,
            IIOService openFileDialogService,            
            IMessageDialogService messageDialogService)
        {
            if (IsInDesignMode)
            {
                //TODO: need to complete design mode
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
                _testCaseEditViewModelCreator = testCaseEditVmCreator;               
                _environmentViewModelCreator = environmentViewModelCreator;
                _openFileDialogService = openFileDialogService;          
                _messageDialogService = messageDialogService;

                SetApplicationVersion();
                TestCaseEditViewModels = new ObservableCollection<ITestCaseEditViewModel>();
                NavigationViewModel = navigationViewModel;
                InitializeCommands();
             
                InitializeEnvironmentSettings();
            }
        }

        private void InitializeEnvironmentSettings()
        {
            EnvironmentConfig = ConfigurationManager.GetSection("Environments") as EnvironmentSettings;

            EnvironmentViewModels = new ObservableCollection<IEnvironmentViewModel>();

            foreach (EnvironmentElement env in EnvironmentConfig.Environments)
            {
                var envVM = _environmentViewModelCreator(env);
                EnvironmentViewModels.Add(envVM);
            }

            var settingsEnv = Properties.Settings.Default.SelectedEnvironment;

            var currentEnvironmentVM = EnvironmentViewModels.SingleOrDefault(x => x.Environment.Name == Properties.Settings.Default.SelectedEnvironment);

            if (currentEnvironmentVM != null)
            {
                currentEnvironmentVM.IsChecked = true;
                SelectedEnvironmentViewModel = currentEnvironmentVM;
            }
        }

        private void SetApplicationVersion()
        {
            ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }        

        private void InitializeCommands()
        {
            CloseTabCommand = new RelayCommand(p => CloseTab(p));
            AddTestCaseCommand = new RelayCommand(p => AddTestCase(p));            
            LoadTestSuiteCommand = new RelayCommand(p => LoadTestCaseSuiteFile());          
            ViewTestCaseSuiteFileCommand = new RelayCommand(p => ViewTestCaseSuiteFile(p));
        }
        
        private void OnEnvironmentSelected(EnvironmentViewModel environmentVm)
        {
            foreach (var envVm in EnvironmentViewModels)
            {
                envVm.IsChecked = false;
            }

            environmentVm.IsChecked = true;
            SelectedEnvironmentViewModel = environmentVm;

            //TODO: probably need to reset all testCaseEditVM's environment, validate if this is working...
            foreach (var testCaseVm in TestCaseEditViewModels)
            {
                testCaseVm.CurrentEnvironment = environmentVm.Environment.Model;
            }

            Properties.Settings.Default.SelectedEnvironment = environmentVm.Environment.Name;
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
                //testCaseEditVm = _testCaseEditViewModelCreator(TestCaseSuiteFile, SelectedEnvironmentViewModel.Environment.Model);
                testCaseEditVm = _testCaseEditViewModelCreator(TestCaseSuiteFile, SelectedEnvironmentViewModel.Environment.Model);
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
            MessageDialogResult result = MessageDialogResult.Yes;

            if (testCaseVm.TestCase.IsChanged)
            {
                result = _messageDialogService.ShowYesNoDialog(string.Format("'{0}' test case changes not saved", testCaseVm.TestCase),
                 "Closing the tab will lose all unsaved changes.\nWould you like to close anyways?", MessageDialogResult.No);
            }

            if (result == MessageDialogResult.Yes)
            {
                TestCaseEditViewModels.Remove(testCaseVm);

                if (TestCaseEditViewModels.Count == 0)
                {
                    SelectedNavigationItemViewModel = null;
                }
            }
        }

        public ICommand LoadTestSuiteCommand { get; private set; }

        private void LoadTestCaseSuiteFile()
        {
            MessageDialogResult dialogResult = MessageDialogResult.Yes;

            if (!string.IsNullOrEmpty(TestCaseSuiteFile))
            {
                dialogResult = ConfirmClosingUnsavedTestCases();
            }

            if (dialogResult == MessageDialogResult.Yes)
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
        }

        public ICommand AddTestCaseCommand { get; private set; }

        private void AddTestCase(object obj)
        {
            ITestCaseEditViewModel testCaseEditVm = _testCaseEditViewModelCreator(TestCaseSuiteFile, SelectedEnvironmentViewModel.Environment.Model);
            TestCaseEditViewModels.Add(testCaseEditVm);
            testCaseEditVm.Load();
            SelectedTestCaseEditViewModel = testCaseEditVm;
        }

        public ICommand ViewTestCaseSuiteFileCommand { get; private set; }

        private void ViewTestCaseSuiteFile(object obj)
        {
            var response = obj as string;
            Process.Start(response);
        }

        #endregion

        internal MessageDialogResult ConfirmClosingUnsavedTestCases()
        {
            var unsavedTestCasesInfo = new StringBuilder();
            int unsavedCount = 0;

            foreach (var testCaseVm in TestCaseEditViewModels)
            {
                if (testCaseVm.TestCase.IsChanged)
                {
                    unsavedCount++;
                    unsavedTestCasesInfo.AppendLine(testCaseVm.TestCase.Name);
                }
            }

            MessageDialogResult result = MessageDialogResult.Yes;

            if (unsavedCount > 0)
            {
                result = _messageDialogService.ShowYesNoDialog("Unsaved Changes",
                string.Format("The following Test Cases have unsaved changes.\nWould you like to close the application without saving your changes?\n\n{0}", unsavedTestCasesInfo.ToString()),
                MessageDialogResult.No);
            }

            return result;
        }
    }
}
