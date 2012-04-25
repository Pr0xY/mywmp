using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MyWMP.Attributes;
using System.Collections.ObjectModel;
using MyWMP.Data;

namespace MyWMP
{
    [Serializable]
    public class Playlist : BaseData
     {

        private string _name = null;
        [Serialize]
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private ObservableCollection<Media> _files;
        public ObservableCollection<Media> Files
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
                NotifyPropertyChanged("Name");
            }
        }

        public Playlist(){ }

    }
}
