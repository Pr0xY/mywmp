using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace MyWMP.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        private readonly CommandBindingCollection _CommandBindings = new CommandBindingCollection();
        public CommandBindingCollection CommandBindings
        {
            get
            {
                return _CommandBindings;
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged("SelectedIndex");
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

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void OpenMedia_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog loadDialog = new Microsoft.Win32.OpenFileDialog();
            loadDialog.Filter = "Tous les fichiers|*.*| Videos (.avi, .wmv, .mpg)|*.avi;*.wmv;*.mpg|Musique (.mp3)|*.mp3| Image (.jpg, .png) | *.jpg; *.png";

            Nullable<bool> result = loadDialog.ShowDialog();
            if (result == true)
            {
                MediaElement mediaCtrl = ((sender as MainWindow).FindName("VideoCtrl") as VideoView).FindName("MediaCtrl") as MediaElement;
                if ((mediaCtrl.DataContext as VideoViewModel).DataMgr.CurrentMediaPlaying != null)
                {
                    //MediaCtrl.Close();
                    if ((mediaCtrl.DataContext as VideoViewModel).SlideMgr.PlayTimer != null)
                        (mediaCtrl.DataContext as VideoViewModel).SlideMgr.PlayTimer.Stop();
                }

                if (SelectedIndex == 0)
                    SelectedIndex = 1;

                (mediaCtrl.DataContext as VideoViewModel).DataMgr.CurrentMediaPlaying = loadDialog.FileName;
                mediaCtrl.Play();
            }
        }

        private void NewPlayList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = "Playlists (*.xml)|.xml";

            Nullable<bool> result = saveDialog.ShowDialog();
            if (result == true)
                (((sender as MainWindow).FindName("PlaylistCtrl") as PlayListView).DataContext as PlayListViewModel).MediaMgr.AddPlayList(saveDialog.SafeFileName.Replace(".xml", ""));
        }

        #endregion

        public MainWindowViewModel()
        {
            PrepareCommandBindings();
        }

        private void PrepareCommandBindings()
        {
            CommandBinding newPlayList = new CommandBinding(ApplicationCommands.New, NewPlayList_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), newPlayList);
            CommandBindings.Add(newPlayList);

            CommandBinding mediaOpened = new CommandBinding(ApplicationCommands.Open, OpenMedia_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), mediaOpened);
            CommandBindings.Add(mediaOpened);

            CommandBinding close = new CommandBinding(ApplicationCommands.Close, Close_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), close);
            CommandBindings.Add(close);
        }
    }
}
