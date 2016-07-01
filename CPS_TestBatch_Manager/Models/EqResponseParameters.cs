using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class EqResponseParameters
    {
        public List<Param> Parameters { get; set; }

        public List<Option> GetListByParamName(string paramName)
        {
            return Parameters.SingleOrDefault(p => p.Name == paramName).Options;
        }

        public List<Option> ResponseChannelOptions
        {
            get { return Parameters.Count > 0 ? GetListByParamName("ResponseChannel") : null; }
        }

        public List<Option> ResponseSatusOptions
        {
            get { return Parameters.Count > 0 ? GetListByParamName("ResponseStatus") : null; }            
        }

        public List<Option> QuestionnaireIdOptions
        {
            get { return Parameters.Count > 0 ? GetListByParamName("QuestionnaireId") : null; }            
        }        
    }
    
    public class Param
    {
        [XmlAttribute()]
        public string Name { get; set; }
        
        public List<Option> Options { get; set; }
    }

    public class Option
    {
        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
