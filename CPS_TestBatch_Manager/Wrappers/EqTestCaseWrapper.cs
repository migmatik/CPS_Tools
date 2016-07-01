using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class EqTestCaseWrapper: ModelWrapper<EqTestCase>
    {
        private EqTestCase _testCase;
        
        public EqSimulatedInputWrapper EQListSimulationInput { get; set; }
        
        public EqTestCaseWrapper(EqTestCase model): base(model)
        {
            _testCase = model;
            InitializeComplexProperties(model);
        }

        private void InitializeComplexProperties(EqTestCase model)
        {
            if (model.EQListSimulationInput == null) { throw new ArgumentException("EQListSimulationInput cannot be null"); }

            EQListSimulationInput = new EqSimulatedInputWrapper(model.EQListSimulationInput);
            RegisterComplexProperty(EQListSimulationInput);
        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string NameOriginalValue { get { return GetOriginalValue<string>("Name"); } }

        public bool NameIsChanged { get { return GetIsChanged("Name"); } }

        public string EqCourierPrefix
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string EqCourierPrefixeOriginalValue { get { return GetOriginalValue<string>("EqCourierPrefix"); } }

        public bool EqCourierPrefixIsChanged { get { return GetIsChanged("EqCourierPrefix"); } }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string DescriptionOriginalValue { get { return GetOriginalValue<string>("Description"); } }

        public bool DescriptionIsChanged { get { return GetIsChanged("Description"); } }


        //public EqSimulatedInputWrapper EqSimulatedInput { get; set; }

        //public ResponseChannel ResponseChannel
        //{
        //    get { return _testCase.EQListSimulationInput.ResponseSettings.ResponseChannel; }
        //    set
        //    {
        //        _testCase.EQListSimulationInput.ResponseSettings.ResponseChannel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public string QuestionnaireId
        //{
        //    get { return _testCase.EQListSimulationInput.ResponseSettings.QuestionnaireId; }
        //    set
        //    {
        //        _testCase.EQListSimulationInput.ResponseSettings.QuestionnaireId = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public string ResponseStatus
        //{
        //    get { return _testCase.EQListSimulationInput.ResponseSettings.ResponseStatus; }
        //    set
        //    {
        //        _testCase.EQListSimulationInput.ResponseSettings.ResponseStatus = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public EqSimulatedInput EqSimulatedInput
        //{
        //    get { return _testCase.EQListSimulationInput; }
        //    set
        //    {
        //        _testCase.EQListSimulationInput = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public List<Response> InputFiles
        //{
        //    get { return _testCase.EQListSimulationInput.Responses; }
        //    set
        //    {
        //        _testCase.EQListSimulationInput.Responses = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public bool IsChanged
        //{
        //    get { return _isChanged; }

        //    private set
        //    {
        //        _isChanged = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public void AcceptChanges()
        //{
        //    //IsChanged = false;
        //    RaisePropertyChanged("IsChanged");
        //}               
       
        public override string ToString()
        {
            return Name;
        }

        //protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.RaisePropertyChanged(propertyName);

        //    // TODO: if or when migrating to C# version 6 (in VS 2015) this satement can be propertyName != nameof(IsChanged) instead of using explicit string value
        //    if (propertyName != "IsChanged")
        //    {
        //        //IsChanged = true;
        //    }
        //}
    }
}
