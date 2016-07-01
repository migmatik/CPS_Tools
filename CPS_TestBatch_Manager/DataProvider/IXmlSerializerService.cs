using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public interface IXmlSerializerService<T> : IDisposable
    {
        //T XmlFileToObject(string file);
        T XmlFileToObject();
        void ObjectToXmlFile(T obj);
    }
}
