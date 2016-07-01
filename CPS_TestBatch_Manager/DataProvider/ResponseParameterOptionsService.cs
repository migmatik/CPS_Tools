using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class ResponseParameterOptionsService : IXmlSerializerService<EqResponseParameters>
    {
        private readonly string _filename = @"Artifacts\EqResponseParameters.xml";        

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public EqResponseParameters XmlFileToObject()
        {
            var ser = new XmlSerializer(typeof(EqResponseParameters));
            EqResponseParameters responseParameters;

            using (XmlReader reader = XmlReader.Create(_filename))
            {
                responseParameters = (EqResponseParameters)ser.Deserialize(reader);
            }

            return responseParameters;
        }


        public void ObjectToXmlFile(EqResponseParameters obj)
        {
            throw new NotImplementedException();
        }
    }
}
