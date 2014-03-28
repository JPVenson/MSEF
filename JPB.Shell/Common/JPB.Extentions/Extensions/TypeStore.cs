using System;
using System.Collections.Generic;

namespace JPB.Extentions.Extensions
{
    [Serializable]
    public class TypeStore
    {
        private List<Type> _typen = new List<Type>();

        public List<Type> Typen
        {
            get { return _typen; }
            set { _typen = value; }
        }
    }
}