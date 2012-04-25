using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWMP.Helper
{
    //gerera tt se qui est serialisation et deserialisation
    public class FileHelper
    {
        private static FileHelper _instance = null;
        public static FileHelper Instance { get { if (_instance == null) _instance = new FileHelper(); return _instance; } }


    }
}
