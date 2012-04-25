using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MyWMP;

namespace MyWMP.ViewModels
{
    public class PlayListViewModel : INotifyPropertyChanged
    {
        #region Properties

        private FileManager _mediaMgr;
        public FileManager MediaMgr
        {
            get { return _mediaMgr; }
            set
            {
                _mediaMgr = value;
                NotifyPropertyChanged("MediaMgr");
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

        public PlayListViewModel()
        {
            MediaMgr = new FileManager();
        }
    }
}
