using CPS_TestBatch_Manager.Models;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class EnvironmentSettingsDataProvider : IXmlSerializerService<EnvironmentSettings>
    {
        private readonly string _filename = @"Artifacts\EnvironmentSettings.xml";

        public EnvironmentSettings XmlFileToObject()
        {
            var ser = new XmlSerializer(typeof(EnvironmentSettings));
            EnvironmentSettings responseParameters;

            using (XmlReader reader = XmlReader.Create(_filename))
            {
                responseParameters = (EnvironmentSettings)ser.Deserialize(reader);
            }

            return responseParameters;
        }

        public void ObjectToXmlFile(EnvironmentSettings obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
