using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class ResponseSettingsWrapper : ModelWrapper<ResponseSettings>
    {        
        public ResponseSettingsWrapper(ResponseSettings model): base(model)
        {            
            InitializeComplexProperties(model);            
        }

        private void InitializeComplexProperties(ResponseSettings model)
        {
            if (model.ResponseChannel == null) { throw new ArgumentException("ResponseChannel cannot be null"); }

            ResponseChannel = new ResponseChannelWrapper(model.ResponseChannel);
            RegisterComplexProperty(ResponseChannel);
        }

        public string QuestionnaireId
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ResponseStatus
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ResponseChannelWrapper ResponseChannel { get; set; }

        //public bool IsChanged
        //{
        //    get { return GetValue<bool>(); }
        //    set { SetValue(value); }
        //}

        //protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.RaisePropertyChanged(propertyName);

        //    // TODO: if or when migrating to C# version 6 (in VS 2015) this satement can be propertyName != nameof(IsChanged) instead of using explicit string value
        //    if (propertyName != "IsChanged")
        //    {
        //        IsChanged = true;
        //    }
        //}
    }
}
