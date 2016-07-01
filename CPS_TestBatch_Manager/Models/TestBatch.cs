using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class TestBatch
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BatchPrefix { get; set; }

        //public string Author { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        public EqSimulatedInput EQListSimulationInput { get; set; }

        //public ResponseChannel ResponseChannel { get; set; }
               
        //public string QuestionnaireId { get; set; }
        
        //public string ResponseStatus { get; set; }                

        //public List<ResponseFile> InputFiles { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }    
}
