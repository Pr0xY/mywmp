using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWMP.Manager;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyWMP.ViewModels
{
    public class VideoViewModel : INotifyPropertyChanged
    {

        #region Properties

        private DataManager dataMgr;
        public DataManager DataMgr
        {
            get { return dataMgr; }
            set
            {
                dataMgr = value;
                NotifyPropertyChanged("DataMgr");
            }
        }

        private SliderManager _slideMgr;

        public SliderManager SlideMgr
        {
            get { return _slideMgr; }
            set
            {
                _slideMgr = value;
                NotifyPropertyChanged("SlideMgr");
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        public VideoViewModel()
        {
            DataMgr = new DataManager();
            SlideMgr = new SliderManager();
        }
    }
}
