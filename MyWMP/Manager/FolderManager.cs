using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using MyWMP.Data;

namespace MyWMP
{
    public class FolderManager
    {
        private static FolderManager _instance = null;
        public static FolderManager Instance { get { if (_instance == null) _instance = new FolderManager(); return _instance; } }


      
    }

}
