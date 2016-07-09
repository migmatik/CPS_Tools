using CPS_TestBatch_Manager.Configuration;
using CPS_TestBatch_Manager.DataProvider;
using CPS_TestBatch_Manager.Events;
using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.Utils;
using CPS_TestBatch_Manager.Views.Dialogs;
using CPS_TestBatch_Manager.Wrappers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.ViewModels
{
    public interface ITestCaseEditViewModel
    {
        EqTestCaseWrapper TestCase { get; set; }
        IEnvironment CurrentEnvironment { get; set; }
        void Load(int? testCaseId = null);
        void Save();
    }

    public class TestCaseEditViewModel : ViewModelBase, ITestCaseEditViewModel
    {
        //TODO: should load this from environment settings, eventually...
        private static readonly string CAT_FAKE_EQRSP_INPUT_DIR = @"\\F7CPA-SVC01\EqResponses\Miguel_Tests\NonTemplateResponses\";
        //private static readonly string CAT_FAKE_EQRSP_INPUT_DIR = @"G:\temp\CPS_TestBatch_Manager_Outputs\";

        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private EqTestCaseWrapper _testCase;
        private IEqTestCaseDataProvider _testCaseDataProvider;
        private ICaseIdDataProvider _caseIdDataProvider;
        private IIOService _openFileDialogService;
        IEnvironment _currentEnvironment;

        public IEnvironment CurrentEnvironment
        {
            get { return _currentEnvironment; }
            set
            {
                _currentEnvironment = value;
                RaisePropertyChanged();
            }
        }

        public EqTestCaseWrapper TestCase
        {
            get { return _testCase; }
            set
            {
                _testCase = value;
                RaisePropertyChanged();
            }
        }

        private ResponseWrapper _selectedResponse;
        public ResponseWrapper SelectedResponse
        {
            get { return _selectedResponse; }
            set
            {
                _selectedResponse = value;
                RaisePropertyChanged();
            }
        }

        public string TestSuiteFile { get; private set; }
        public EqResponseParameters ResponseParameterOptions { get; private set; }
        public INavigationViewModel NavigationViewModel { get; set; }

        private string _eqCourierIdCreated;

        public string EqCourierIdCreated
        {
            get { return _eqCourierIdCreated; }
            set
            {
                _eqCourierIdCreated = value;
                RaisePropertyChanged();
            }
        }

        public TestCaseEditViewModel(
            string filename, IEnvironment currentEnvironment, Func<string, IEqTestCaseDataProvider> testCaseDataProviderCreator,
            IXmlSerializerService<EqResponseParameters> responseParamOptionsDataprovider,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ICaseIdDataProvider caseIdDataProvider,
            IIOService openFileDialogService)
        {
            if (testCaseDataProviderCreator == null) { throw new ArgumentNullException("testCaseDataProvider"); }

            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _testCaseDataProvider = testCaseDataProviderCreator(filename);
            _caseIdDataProvider = caseIdDataProvider;
            _openFileDialogService = openFileDialogService;
            ResponseParameterOptions = responseParamOptionsDataprovider.XmlFileToObject();
            TestSuiteFile = filename;
            _currentEnvironment = currentEnvironment;
            AddResponseCommand = new RelayCommand(p => AddResponse(p));
            RemoveResponseCommand = new RelayCommand(p => RemoveResponse(p));
            InitializeCommands();
        }

        public void Load(int? testCaseId = null)
        {
            var testCase = testCaseId.HasValue ? _testCaseDataProvider.GetEqTestCaseById(testCaseId.Value) : GetNewTestCase();
            TestCase = new EqTestCaseWrapper(testCase);
        }

        private EqTestCase GetNewTestCase()
        {
            return new EqTestCase
            {
                EQListSimulationInput = new EqSimulatedInput
                {
                    ResponseSettings = new ResponseSettings { ResponseChannel = new ResponseChannel() },
                    Responses = new List<Response>()
                },
                //TODO: should get this from config file or something
                EqCourierPrefix = "SIM_"
            };
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(p => Save(), OnSaveCanExecute);
            ResetCommand = new RelayCommand(p => ResetChanges(), OnSaveCanExecute);
            DeleteCommand = new RelayCommand(p => Delete());
            RunCommand = new RelayCommand(p => RunTestCase());
            NavigateToLinkCommand = new RelayCommand(p => GoToLink(p));
        }

        private bool OnSaveCanExecute(object obj)
        {
            return TestCase.IsChanged;
        }

        private List<string> _questionnaireIdList;

        public List<string> QuestionnaireIdList
        {
            get
            {
                if (_questionnaireIdList == null)
                {
                    _questionnaireIdList = ResponseParameterOptions.QuestionnaireIdOptions.Select(x => x.Value).ToList();
                }

                return _questionnaireIdList;
            }
            set
            {
                _questionnaireIdList = value;
            }
        }

        private string _selectedQuestionnaireId;

        public string SelectedQuestionnaireId
        {
            get
            {
                if (_selectedQuestionnaireId == null)
                {
                    _selectedQuestionnaireId = TestCase.EQListSimulationInput.ResponseSettings.QuestionnaireId;
                }

                return _selectedQuestionnaireId;
            }

            set
            {
                _selectedQuestionnaireId = value;
                TestCase.EQListSimulationInput.ResponseSettings.QuestionnaireId = value;
                RaisePropertyChanged();
            }
        }

        private List<ResponseChannel> _responseChannelList;

        public List<ResponseChannel> ResponseChannelList
        {
            get
            {
                if (_responseChannelList == null)
                {
                    _responseChannelList = ResponseParameterOptions.ResponseChannelOptions
                        .Select(x => new ResponseChannel { Id = x.Id, Value = x.Value }).ToList();
                }

                return _responseChannelList;
            }
            set
            {
                _responseChannelList = value;
                RaisePropertyChanged();
            }
        }

        private ResponseChannel _selectedResponseChannel;

        public ResponseChannel SelectedResponseChannel
        {
            get
            {
                if (_selectedResponseChannel == null)
                {
                    _selectedResponseChannel = ResponseChannelList.SingleOrDefault(x => x.Id == TestCase.EQListSimulationInput.ResponseSettings.ResponseChannel.Id);
                }

                return _selectedResponseChannel;
            }
            set
            {
                _selectedResponseChannel = value;
                TestCase.EQListSimulationInput.ResponseSettings.ResponseChannel.Id = value.Id;
                TestCase.EQListSimulationInput.ResponseSettings.ResponseChannel.Value = value.Value;
                RaisePropertyChanged();
            }
        }

        private List<string> _responseStatusList;

        public List<string> ResponseStatusList
        {
            get
            {
                if (_responseStatusList == null)
                {
                    _responseStatusList = ResponseParameterOptions.ResponseSatusOptions.Select(x => x.Value).ToList();
                }

                return _responseStatusList;
            }
            set
            {
                _responseStatusList = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedResponseStatus;

        public string SelectedResponseStatus
        {
            get
            {
                if (string.IsNullOrEmpty(_selectedResponseStatus))
                {
                    _selectedResponseStatus = ResponseStatusList.FirstOrDefault(x => x == TestCase.EQListSimulationInput.ResponseSettings.ResponseStatus);
                }

                return _selectedResponseStatus;
            }
            set
            {
                _selectedResponseStatus = value;
                TestCase.EQListSimulationInput.ResponseSettings.ResponseStatus = value;
                RaisePropertyChanged();
            }
        }

        private OperationResult CreateResponseFileForFakeEqRspService(string packageFolder)
        {
            var result = new OperationResult { Success = false };

            if (AllResonseFilesExist(TestCase.EQListSimulationInput.Responses))
            {
                GenerateCaseIdAndResponseIdIfNull();

                Directory.CreateDirectory(packageFolder);

                foreach (var response in TestCase.EQListSimulationInput.Responses)
                {
                    //File.Copy(response.ResponseFile, string.Concat(CAT_FAKE_EQRSP_INPUT_DIR, response.ResponseId, ".xml"), true);
                    File.Copy(response.ResponseFile, Path.Combine(CurrentEnvironment.OutputFolder, string.Concat(response.ResponseId, ".xml")), true);
                    File.Copy(response.ResponseFile, Path.Combine(packageFolder, string.Concat(response.ResponseId, ".xml")), true);
                }

                result.Success = true;
            }

            return result;
        }

        private bool AllResonseFilesExist(ObservableCollection<ResponseWrapper> observableCollection)
        {
            bool allFileExist = true;
            var responseFilesNotFound = new StringBuilder();

            foreach (var response in TestCase.EQListSimulationInput.Responses)
            {
                if (!File.Exists(response.ResponseFile))
                {
                    responseFilesNotFound.AppendLine(response.ResponseFile);
                    allFileExist = false;
                }
            }

            if (!allFileExist)
            {
                var result = _messageDialogService.ShowOkDialog("Response File Not Found",
                string.Format("Test Run aborted because the following file(s) were not found{0}{1}", System.Environment.NewLine, responseFilesNotFound.ToString()),
                MessageDialogResult.Ok);
            }

            return allFileExist;
        }

        private void CreateInputFileForSimulationEqList(string outputDir, string suffix)
        {
            var ser = new XmlSerializer(TestCase.Model.EQListSimulationInput.GetType(), "");
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            string filename = Path.Combine(outputDir, string.Concat(TestCase.Model.EqCourierPrefix, TestCase.Model.Name, "_", suffix, ".xml"));

            using (StreamWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, TestCase.Model.EQListSimulationInput, ns);
            }
        }

        private void GenerateCaseIdAndResponseIdIfNull()
        {
            foreach (var resp in TestCase.EQListSimulationInput.Responses)
            {
                if (string.IsNullOrEmpty(resp.CaseId))
                {
                    resp.CaseId = _caseIdDataProvider.GetAvailableCaseId(TestCase.EQListSimulationInput.ResponseSettings.QuestionnaireId);
                }

                if (string.IsNullOrEmpty(resp.ResponseId))
                {
                    resp.ResponseId = string.Concat("11", resp.CaseId);
                }
            }
        }

        private void CreateEqInitInputFile(string testCaseOutputDir, string suffix)
        {
            EqInitInputFileCreator.Create(TestCase, CurrentEnvironment.EQInitInputFolder, testCaseOutputDir, suffix);
            var eqCourier = string.Concat(TestCase.EqCourierPrefix, suffix);
            EqCourierIdCreated = string.Concat(eqCourier, Helper.GetMAD97(eqCourier));
        }

        #region Commands
        public ICommand NavigateToLinkCommand { get; set; }

        private void GoToLink(object obj)
        {
            var response = obj as ResponseWrapper;

            if (!File.Exists(response.ResponseFile))
            {
                _messageDialogService.ShowOkDialog("File Not Found", string.Format("Unable to find file {0}", response.ResponseFile), MessageDialogResult.Ok);
                return;
            }

            Process.Start(response.ResponseFile);
        }

        public ICommand SaveCommand { get; private set; }

        public void Save()
        {
            _testCaseDataProvider.SaveEqTestCase(TestCase.Model);
            TestCase.AcceptChanges();
            _eventAggregator.GetEvent<TestCaseSavedEvent>().Publish(TestCase.Model);
        }

        public ICommand AddResponseCommand { get; private set; }

        private void AddResponse(object obj)
        {
            string responseFile = _openFileDialogService.OpenFileDialog();

            if (!string.IsNullOrEmpty(responseFile))
            {
                TestCase.EQListSimulationInput.Responses.Add(new ResponseWrapper(new Response { ResponseFile = responseFile }));
            }
        }

        public ICommand RemoveResponseCommand { get; private set; }

        private void RemoveResponse(object obj)
        {
            TestCase.EQListSimulationInput.Responses.Remove(SelectedResponse);
        }

        public ICommand ResetCommand { get; private set; }

        private void ResetChanges()
        {
            TestCase.RejectChanges();
            var selectedResponseChannel = ResponseChannelList.SingleOrDefault(r => r.Id == TestCase.EQListSimulationInput.ResponseSettings.ResponseChannel.Id);

            if (selectedResponseChannel != null)
            {
                SelectedResponseChannel = selectedResponseChannel;
            }
        }

        public ICommand DeleteCommand { get; private set; }

        private void Delete()
        {
            var result = _messageDialogService.ShowYesNoDialog("Delete Test Case",
                string.Format("Are you sure you want to delete Test Case '{0}'", TestCase),
                MessageDialogResult.No);

            if (result == MessageDialogResult.Yes)
            {
                _testCaseDataProvider.DeleteEqTestCase(TestCase.Id);
                _eventAggregator.GetEvent<TestCaseDeletedEvent>().Publish(TestCase.Id);
            }
        }

        public ICommand RunCommand { get; private set; }

        private void RunTestCase()
        {
            string batchSuffix = EqInitInputFileCreator.GenerateBatchSuffix();
            var testCaseOutputDir = Path.Combine(Path.GetDirectoryName(this.TestSuiteFile), string.Concat(TestCase.Name, "_", batchSuffix));

            if (CreateResponseFileForFakeEqRspService(testCaseOutputDir).Success)
            {
                CreateInputFileForSimulationEqList(testCaseOutputDir, batchSuffix);
                CreateEqInitInputFile(testCaseOutputDir, batchSuffix);
            }
        }

        #endregion
    }
}
