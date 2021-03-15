using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoWorkFlow.Serializable
{
    public class XmlSerializerHelper<T>
    {
        public XmlSerializerHelper()
        {

        }

        public void Serialize(string path, T data, FileMode mode = FileMode.OpenOrCreate)
        {         

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (FileStream stream = new FileStream(path, mode))
            {
                xmlSerializer.Serialize(stream, data);
            }
        }

        public T Deserialize(string path, FileMode mode = FileMode.OpenOrCreate)
        {
            T data;
           

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (FileStream stream = new FileStream(path, mode))
            {
                data = (T)xmlSerializer.Deserialize(stream);
            }
            return data;            
        }


    }
}
