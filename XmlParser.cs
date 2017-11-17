using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicMetadataOrganizer
{
    public static class XmlParser
    {
        // Try to change this to not return a list, but just an object
        public static List<RESPONSE> XmlToObject(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<RESPONSE>), new XmlRootAttribute("RESPONSES"));
            using (var stringReader = new StringReader(xml))
            {
                return (List<RESPONSE>)serializer.Deserialize(stringReader);
            }
        }
    }
}
