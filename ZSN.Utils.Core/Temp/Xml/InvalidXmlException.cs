using System;
using System.Text;

namespace Yannyo.Common.Xml
{
    public class InvalidXmlException : HTTP_Exception
    {
        public InvalidXmlException(string message)
            : base(message)
        {
        }
    }
}
