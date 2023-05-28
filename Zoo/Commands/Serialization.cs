using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Zoo
{
    public class ListOfICommand: List<ICommand>, IXmlSerializable
    {
        // https://stackoverflow.com/questions/3704807/xmlserializer-serialize-generic-list-of-interface
        public ListOfICommand() : base() { }

        #region IXmlSerializable
        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("ListOfICommand");
            while (reader.IsStartElement("ICommand"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("ICommand");
                this.Add((ICommand)serial.Deserialize(reader));
                reader.ReadEndElement(); 
            }
            reader.ReadEndElement(); 
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (ICommand command in this)
            {
                writer.WriteStartElement("ICommand");
                writer.WriteAttributeString("AssemblyQualifiedName", command.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(command.GetType());
                xmlSerializer.Serialize(writer, command);
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
