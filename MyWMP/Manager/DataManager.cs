using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyWMP.Manager
{
    public class DataManager : INotifyPropertyChanged
    {
        #region Properties


        private string _currentMediaPlaying;
        public string CurrentMediaPlaying
        {
            get
            {
                return _currentMediaPlaying;
            }
            set
            {
                _currentMediaPlaying = value;
                NotifyPropertyChanged("CurrentMediaPlaying");
            }
        }

        private string _mediaLength = "00:00:00";
        public string MediaLength
        {
            get
            {
                return _mediaLength;
            }
            set
            {
                _mediaLength = value;
                NotifyPropertyChanged("MediaLength");
            }
        }

        private TimeSpan _mediaPosition;
        public TimeSpan MediaPosition
        {
            get { return _mediaPosition; }
            set
            {
                _mediaPosition = value;
                NotifyPropertyChanged("SliderPosition");
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

        public DataManager()
        {
        }
    }
}
