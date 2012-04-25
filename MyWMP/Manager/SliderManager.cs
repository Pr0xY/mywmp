using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.ComponentModel;

namespace MyWMP.Manager
{
    public class SliderManager : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        private DispatcherTimer _playtimer = new DispatcherTimer();
        public DispatcherTimer PlayTimer
        {
            get { return _playtimer; }
            set { _playtimer = value; }
        }

        private double _maximumDuration;
        public double MaximumDuration
        {
            get { return _maximumDuration; }
            set
            {
                _maximumDuration = value;
                NotifyPropertyChanged("MaximumDuration");
            }
        }

        private int _sliderSmallChange = 1;
        public int SliderSmallChange
        {
            get { return _sliderSmallChange; }
            set
            {
                _sliderSmallChange = value;
                NotifyPropertyChanged("SliderSmallChange");
            }
        }

        private int _sliderLargeChange = 1;
        public int SliderLargeChange
        {
            get { return _sliderLargeChange; }
            set
            {
                _sliderLargeChange = value;
                NotifyPropertyChanged("SliderLargeChange");
            }
        }

        private double _sliderValue = 0;
        public double SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                NotifyPropertyChanged("SliderValue");
            }
        }

        private TimeSpan _sliderTest;

        public TimeSpan SliderTest
        {
            get { return _sliderTest; }
            set { _sliderTest = value; NotifyPropertyChanged("SliderTest"); }
        }
       
        private bool _isDragging;
        public bool IsDragging
        {
            get { return _isDragging; }
            set { _isDragging = value; }
        }

        private bool _isClicked;
        public bool IsClicked
        {
            get { return _isClicked; }
            set { _isClicked = value; }
        }

        public SliderManager()
        {
        }
    }
}
