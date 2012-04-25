using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWMP.Attributes
{
    class SerializeAttribute : Attribute
    {
        private bool _isCollection;
        public bool IsCollection
        {
            get { return _isCollection; }
            set { _isCollection = value; }
        }

        public SerializeAttribute()
        {
        }

        public SerializeAttribute(bool isCollection)
        {
            IsCollection = isCollection;
        }
    }
}
