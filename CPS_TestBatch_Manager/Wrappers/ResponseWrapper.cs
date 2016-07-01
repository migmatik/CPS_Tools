using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class ResponseWrapper: ModelWrapper<Response>
    {
        public ResponseWrapper(Response model): base(model)
        {
            
        }

        public string CaseId
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string CaseIdOriginalValue 
        { 
            get { return GetOriginalValue<string>("CaseId"); } 
        }
        
        public bool CaseIdIsChanged
        {
            get { return GetIsChanged("CaseId"); }
        }

        public string ResponseId
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ResponseIdOriginalValue
        {
            get{ return GetOriginalValue<string>("ResponseId"); }
        }

        public bool ResponseIdIsChanged
        {
            get { return GetIsChanged("ResponseId"); }
        }

        public string ResponseFile
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ResponseFileOriginalValue
        {
            get{ return GetOriginalValue<string>("ResponseFile"); }
        }

        public bool ResponseFileIsChanged
        {
            get { return GetIsChanged("ResponseFile"); }
        }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrWhiteSpace(CaseId))
        //    {
        //        yield return new ValidationResult("CaseId is required",
        //          new[] { "CaseId" });
        //    }
        //    //if (IsDeveloper && Emails.Count == 0)
        //    //{
        //    //    yield return new ValidationResult("A developer must have an email-address",
        //    //      new[] { nameof(IsDeveloper), nameof(Emails) });
        //    //}
        //}
    }
}
