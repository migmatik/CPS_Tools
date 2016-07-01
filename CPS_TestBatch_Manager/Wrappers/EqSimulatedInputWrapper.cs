using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class EqSimulatedInputWrapper : ModelWrapper<EqSimulatedInput>
    {
        public ResponseSettingsWrapper ResponseSettings { get; set; }
        
        public ChangeTrackingCollection<ResponseWrapper> Responses { get; set; }

        public EqSimulatedInputWrapper(EqSimulatedInput model): base(model)
        {            
            InitializeComplexProperties(model);
            
            InitializeCollectionProperties(model);
        }

        private void InitializeCollectionProperties(EqSimulatedInput model)
        {
            Responses = new ChangeTrackingCollection<ResponseWrapper>(model.Responses.Select(r => new ResponseWrapper(r)));
            RegisterCollection(Responses, model.Responses);
        }

        private void InitializeComplexProperties(EqSimulatedInput model)
        {
            if (model.ResponseSettings == null) { throw new ArgumentException("ResponseSettings cannot be null"); }

            ResponseSettings = new ResponseSettingsWrapper(model.ResponseSettings);
            RegisterComplexProperty(ResponseSettings);
        }
    }
}
