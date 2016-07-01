using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.Services;
using CPS_TestBatch_Manager.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CPS_TestBatch_Manager.ViewModels
{
    public interface ITestBatchEditViewModel
    {
        void Load(int? testBatchId);

        TestBatchWrapper TestBatch { get; }

        void Load(TestBatch selectedTestBatch);

        void SetTestBatchSuite(TestBatchSuite TestBatchSuiteFile);
    }

    public class TestBatchEditViewModel: ViewModelBase, ITestBatchEditViewModel    
    {
        //TODO: should load this from environment settings, eventually...
        private static readonly string CAT_FAKE_RQRSP_INPUT_DIR = @"\\F7CPA-SVC01\EqResponses\Miguel_Tests\NonTemplateResponses\";

        private ITestBatchDataService _dataProvider;

        public TestBatchSuite TestBatchSuite { get; private set; }

        public string TestBatchSuiteFileDirectory 
        { 
            get
            {
                return Path.GetDirectoryName(TestBatchSuite.FileName);
            }
        }


        public TestBatchEditViewModel(ITestBatchDataService dataProvider)
        {
            if (dataProvider == null) { throw new ArgumentNullException("dataProvider not instanciated"); }

            _dataProvider = dataProvider;            
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if(_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        p => Save(),
                        p => { return TestBatch.IsChanged; }
                        );
                }
                return _saveCommand;
            }                       
        }

        private void Save()
        {
            _dataProvider.SaveTestBatchSuite(TestBatch.TestBatchSuite);
            TestBatch.AcceptChanges();            
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if(_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(
                        p => Delete(),
                        p => { return true; }
                        );
                }
                return _deleteCommand;
            }
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }

        private ICommand _runCommand;
        public ICommand RunCommand
        {
            get
            {
                if (_runCommand == null)
                {
                    _runCommand = new RelayCommand(
                        p => Run(),
                        p => { return true; }
                        );
                }
                return _runCommand;
            }
        }

        private void Run()
        {
            string batchSuffix = EqInitInputFileCreator.GenerateBatchSuffix();

            CreateInputFileForSimulationEqList(TestBatchSuiteFileDirectory, batchSuffix);
            CreateResponseFileForFakeEqRspService();
            CreateEqInitInputFile(TestBatchSuiteFileDirectory, batchSuffix);
        }

        private void CreateResponseFileForFakeEqRspService()
        {
            foreach(var response in TestBatch.EqSimulatedInput.Responses)
            {
                File.Copy(response.ResponseFile, string.Concat(CAT_FAKE_RQRSP_INPUT_DIR, response.ResponseId, ".xml"), true);
            }
        }

        private void CreateInputFileForSimulationEqList(string outputDir,string suffix)
        {
            GenerateCaseIdAndResponseIdIfNull();
            _dataProvider.SaveTestBatch(TestBatch.Model, outputDir, suffix);
            //_dataProvider.SaveTestBatch(TestBatch.Model, outputDir, timestamp);
        }

        //TODO: get the case id from the proper place, such as a file...
        private void GenerateCaseIdAndResponseIdIfNull()
        {
            int i = 1;
            foreach(var resp in TestBatch.Model.EQListSimulationInput.Responses)
            {
                if(string.IsNullOrEmpty(resp.CaseId))
                {
                    resp.CaseId = string.Format("{0}2345", i);
                    i++;
                }

                if (string.IsNullOrEmpty(resp.ResponseId))
                {
                    resp.ResponseId = string.Concat("11", resp.CaseId);
                }
            }
        }

        private void CreateEqInitInputFile(string outputDir, string suffix)
        {
            EqInitInputFileCreator.Create(TestBatch, outputDir, suffix);
        }  

        public void Load(int? testBatchId)
        {
            var testBatch = testBatchId.HasValue ? _dataProvider.GetTestBatchById(testBatchId.Value) : new TestBatch();
            TestBatch = new TestBatchWrapper(testBatch, TestBatchSuite);
        }

        public void Load(TestBatch testBatch)
        {
            TestBatch = new TestBatchWrapper(testBatch, TestBatchSuite);            
        }       
       
        private TestBatchWrapper _testBatch;

        public TestBatchWrapper TestBatch
        {
            get { return _testBatch; }
            set
            {
                _testBatch = value;
                RaisePropertyChanged();
            }
        }

        public void SetTestBatchSuite(TestBatchSuite testBatchSuite)
        {
            TestBatchSuite = testBatchSuite;
        }
    }
}
