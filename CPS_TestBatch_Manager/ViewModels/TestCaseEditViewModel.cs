using CPS_TestBatch_Manager.DataProvider;
using CPS_TestBatch_Manager.DataProvider.Lookups;
using CPS_TestBatch_Manager.Events;
using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.Views.Dialogs;
using CPS_TestBatch_Manager.Wrappers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.ViewModels
{
    public interface ITestCaseEditViewModel
    {
        EqTestCaseWrapper TestCase { get; set; }

        void Load(int? testCaseId = null);
    }
    public class TestCaseEditViewModel: ViewModelBase, ITestCaseEditViewModel 
    {
        //TODO: should load this from environment settings, eventually...
        //private static readonly string CAT_FAKE_EQRSP_INPUT_DIR = @"\\F7CPA-SVC01\EqResponses\Miguel_Tests\NonTemplateResponses\";
        private static readonly string CAT_FAKE_EQRSP_INPUT_DIR = @"G:\temp\CPS_TestBatch_Manager_Outputs\";

        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private EqTestCaseWrapper _testCase;               
        private IEqTestCaseDataProvider _testCaseDataProvider;
        private ICaseIdDataProvider _caseIdDataProvider;
 
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
                //((DelegateCommand)RemoveEmailCommand).RaiseCanExecuteChanged();
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
            string filename, Func<string, IEqTestCaseDataProvider> testCaseDataProviderCreator,
            IXmlSerializerService<EqResponseParameters> responseParamOptionsDataprovider, 
            IEventAggregator eventAggregator, 
            IMessageDialogService messageDialogService,
            ICaseIdDataProvider caseIdDataProvider)
        {
            if (testCaseDataProviderCreator == null) { throw new ArgumentNullException("testCaseDataProvider"); }

            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _testCaseDataProvider = testCaseDataProviderCreator(filename);
            _caseIdDataProvider = caseIdDataProvider;
            ResponseParameterOptions = responseParamOptionsDataprovider.XmlFileToObject();
            TestSuiteFile = filename;
            InitializeCommands();
        }
       
        public void Load(int? testCaseId = null)
        {
            var testCase = testCaseId.HasValue ? _testCaseDataProvider.GetEqTestCaseById(testCaseId.Value) : GetNewTestCase();

            TestCase = new EqTestCaseWrapper(testCase);

            //TestCase.PropertyChanged += (s, e) =>
            //    {
            //        if(e.PropertyName == "IsChanged")
            //        {
            //            InvalidateCommands();
            //        }

            //        InvalidateCommands();
            //    };
        }

        private void InvalidateCommands()
        {
            //((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            //((RelayCommand)ResetCommand).RaiseCanExecuteChanged();
            //((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        private EqTestCase GetNewTestCase()
        {
            throw new NotImplementedException();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(p => Save(), OnSaveCanExecute);
            ResetCommand = new RelayCommand(p => ResetChanges());
            DeleteCommand = new RelayCommand(p => Delete());
            RunCommand = new RelayCommand(p => RunTestCase());
            NavigateToLinkCommand = new RelayCommand(p => GoToLink(p));
        }        

        private bool OnSaveCanExecute(object obj)
        {
            return TestCase.IsChanged;            
        }

        private void GoToLink(object obj)
        {
            var response = obj as ResponseWrapper;

            //TODO: show dialog message when file is not found

            Process.Start(response.ResponseFile);
        }
                
        private List<string> _questionnaireIdList;

        public List<string> QuestionnaireIdList
        {
            get
            {
                if( _questionnaireIdList == null)
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
                if(_selectedQuestionnaireId == null)
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
                if(_responseChannelList == null)
                {
                    _responseChannelList = ResponseParameterOptions.ResponseChannelOptions
                        .Select(x => new ResponseChannel{ Id = x.Id, Value = x.Value }).ToList();                    
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

        public ICommand NavigateToLinkCommand { get; set; }
        
        public ICommand SaveCommand { get; private set; }
       
        private void Save()
        {            
            _testCaseDataProvider.SaveEqTestCase(TestCase.Model);
            TestCase.AcceptChanges();
            _eventAggregator.GetEvent<TestCaseSavedEvent>().Publish(TestCase.Model);
        }

        public ICommand ResetCommand { get; private set; }

        private void ResetChanges()
        {
            TestCase.RejectChanges();
            SelectedResponseChannel = ResponseChannelList.Single(r => r.Id == TestCase.EQListSimulationInput.ResponseSettings.ResponseChannel.Id);
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

            //CreateInputFileForSimulationEqList(TestBatchSuiteFileDirectory, batchSuffix);
            CreateInputFileForSimulationEqList(@"G:\temp\CPS_TestBatch_Manager_Outputs", batchSuffix);
            CreateResponseFileForFakeEqRspService();
            //CreateEqInitInputFile(TestBatchSuiteFileDirectory, batchSuffix);
            CreateEqInitInputFile(@"G:\temp\CPS_TestBatch_Manager_Outputs", batchSuffix);
        }

        private void CreateResponseFileForFakeEqRspService()
        {
            foreach (var response in TestCase.EQListSimulationInput.Responses)
            {
                File.Copy(response.ResponseFile, string.Concat(CAT_FAKE_EQRSP_INPUT_DIR, response.ResponseId, ".xml"), true);
            }
        }

        private void CreateInputFileForSimulationEqList(string outputDir, string suffix)
        {
            GenerateCaseIdAndResponseIdIfNull();
            SaveTestBatch(TestCase.Model, outputDir, suffix);
            //_testCaseDataProvider.SaveTestBatch(TestCase.Model, outputDir, suffix);
            //_dataProvider.SaveTestBatch(TestBatch.Model, outputDir, timestamp);
        }

        public void SaveTestBatch(EqTestCase testCase, string outDir, string suffix)
        {
            var ser = new XmlSerializer(testCase.EQListSimulationInput.GetType(), "");
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            string filename = Path.Combine(outDir, string.Concat(testCase.EqCourierPrefix, testCase.Name, "_", suffix, ".xml"));

            using (StreamWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, testCase.EQListSimulationInput, ns);
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

        private void CreateEqInitInputFile(string outputDir, string suffix)
        {
            EqInitInputFileCreator.Create(TestCase, outputDir, suffix);
            EqCourierIdCreated = string.Concat(TestCase.EqCourierPrefix, suffix);
        }        
    }
}
