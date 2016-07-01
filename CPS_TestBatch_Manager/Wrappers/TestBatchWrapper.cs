using CPS_TestBatch_Manager.Framework;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CPS_TestBatch_Manager.Wrappers
{    
    public class TestBatchWrapper : ModelWrapper<TestBatch>
    {
        private TestBatchSuite _testBatchSuite;
        private TestBatch _testBatch;
        private bool _isChanged;
        
        public TestBatchWrapper(TestBatch model): base(model)
        { }

        public TestBatchWrapper(TestBatch model, TestBatchSuite testBatchSuite): base(model)
        {
            _testBatch = model;
            _testBatchSuite = testBatchSuite;
        }

        public TestBatchSuite TestBatchSuite 
        {
            get { return _testBatchSuite; }            
        }
       
        public bool IsChanged
        {
            get 
            { 
                return _isChanged; 
            }

            private set
            {
                _isChanged = value;
                RaisePropertyChanged();
            }
        }

        public void AcceptChanges()
        {
            IsChanged = false;
        }

        public int Id
        {
            get { return _testBatch.Id; }
            set
            {
                _testBatch.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name 
        { 
            get{ return _testBatch.Name; }            
            set
            {
                _testBatch.Name = value;
                RaisePropertyChanged();
            }
        }

        public string BatchPrefix
        {
            get { return _testBatch.BatchPrefix; }
            set
            {
                _testBatch.BatchPrefix = value;
                RaisePropertyChanged();
            }
        }

        //public string Author { get; set; }

        public DateTime CreationDate
        {
            get { return _testBatch.CreationDate; }
            set
            {
                _testBatch.CreationDate = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return _testBatch.Description; }
            set
            {
                _testBatch.Description = value;
                RaisePropertyChanged();
            }
        }
        
        public ResponseChannel ResponseChannel
        {
            get { return _testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel; }
            set
            {
                _testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel = value;
                RaisePropertyChanged();
            }
        }
        
        public string QuestionnaireId
        {
            get { return _testBatch.EQListSimulationInput.ResponseSettings.QuestionnaireId; }
            set
            {
                _testBatch.EQListSimulationInput.ResponseSettings.QuestionnaireId = value;
                RaisePropertyChanged();
            }
        }
       
        public string ResponseStatus
        {
            get { return _testBatch.EQListSimulationInput.ResponseSettings.ResponseStatus; }
            set
            {
                _testBatch.EQListSimulationInput.ResponseSettings.ResponseStatus = value;
                RaisePropertyChanged();
            }
        }

        public EqSimulatedInput EqSimulatedInput
        {
            get { return _testBatch.EQListSimulationInput; }
            set
            {
                _testBatch.EQListSimulationInput = value;
                RaisePropertyChanged();
            }
        }        

        public List<Response> InputFiles
        {
            get { return _testBatch.EQListSimulationInput.Responses; }
            set
            {                
                _testBatch.EQListSimulationInput.Responses = value;
                RaisePropertyChanged();                          
            }
        }        
        
        public string SelectedQuestionnaireId
        {
            get { return _testBatch.EQListSimulationInput.ResponseSettings.QuestionnaireId; }
            set
            {
                if (_testBatch.EQListSimulationInput.ResponseSettings.QuestionnaireId != value)
                {
                    _testBatch.EQListSimulationInput.ResponseSettings.QuestionnaireId = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ResponseChannel SelectedResponseChannel
        {
            get 
            {
                return _testBatchSuite.ResponseChannelList
                    .SingleOrDefault(x => x.Id == _testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel.Id 
                        && x.Value == _testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel.Value);                
            }
            set
            {
                if (_testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel != value)
                {
                    _testBatch.EQListSimulationInput.ResponseSettings.ResponseChannel = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedResponseStatus
        {
            get { return _testBatch.EQListSimulationInput.ResponseSettings.ResponseStatus; }
            set
            {
                if (_testBatch.EQListSimulationInput.ResponseSettings.ResponseStatus != value)
                {
                    _testBatch.EQListSimulationInput.ResponseSettings.ResponseStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }


        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);


            // TODO: if or when migrating to C# version 6 (in VS 2015) this satement can be propertyName != nameof(IsChanged) instead of using explicit string value
            if(propertyName != "IsChanged")
            {
                IsChanged = true;
            }
        }
    }
}
