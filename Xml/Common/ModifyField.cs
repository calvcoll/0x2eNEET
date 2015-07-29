using System;
using System.Xml.Serialization;

namespace DotNEET.Xml
{
    [Serializable]
    public class ModifyField
    {
        [XmlElement("FieldName")]
        public string FieldName;

        [XmlElement("FieldValue")]
        public string FieldValue;
    }
}