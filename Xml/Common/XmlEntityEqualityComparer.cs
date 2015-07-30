using System.Collections.Generic;

namespace DotNEET.Xml
{
    public class XmlEntityEqualityComparer : IEqualityComparer<XmlEntity>
    {
        public XmlEntityEqualityComparer()
        {
        }

        public bool Equals(XmlEntity x, XmlEntity y)
        {
            if (object.ReferenceEquals(x, y)) // if val == val or null == null
            {
                return true;
            }
            //no null check here, mabe add later
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(XmlEntity obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return 0;
            }
            return obj.Id.GetHashCode();
        }
    }
}