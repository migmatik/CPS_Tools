using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class FileToPocoSerializer<T>: IXmlSerializerService<T>
    {
        private string _filename;

        public FileToPocoSerializer(string filename)
        {
            _filename = filename;
        }

        //public T XmlFileToObject(string file)
        //{
        //    var ser = new XmlSerializer(typeof(T));
        //    T poco;

        //    using (XmlReader reader = XmlReader.Create(file))
        //    {
        //        poco = (T)ser.Deserialize(reader);
        //    }

        //    return poco;
        //}

        public T XmlFileToObject()
        {
            var ser = new XmlSerializer(typeof(T));
            T poco;

            using (XmlReader reader = XmlReader.Create(_filename))
            {
                poco = (T)ser.Deserialize(reader);
            }

            return poco;
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        public void ObjectToXmlFile(T obj)
        {
            var ser = new XmlSerializer(typeof(T), "");
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (StreamWriter writer = new StreamWriter(_filename))
            {
                ser.Serialize(writer, obj, ns);
            }  
        }
    }
}
