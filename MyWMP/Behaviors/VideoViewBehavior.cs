using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;
using MyWMP.ViewModels;
using System.Windows;

namespace MyWMP.Behaviors
{
    public class PauseBehavior : Behavior<Button>
    {
        private VideoViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            AssociatedObject.Click += new System.Windows.RoutedEventHandler(AssociatedObject_Click);
        }

        void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel.DataMgr.CurrentMediaPlaying != null)
            {
                (AssociatedObject.FindName("MediaCtrl") as MediaElement).Pause();
                ViewModel.SlideMgr.PlayTimer.Stop();
            }
        }
    }

    public class FullScreenBehavior : Behavior<Button>
    {
        private bool _fullscreen;
        private VideoViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            AssociatedObject.Click += new System.Windows.RoutedEventHandler(AssociatedObject_Click);
        }

        void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel.DataMgr.CurrentMediaPlaying != null)
            {
                if (!_fullscreen)
                {
                    Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;

                }
                else
                {
                    Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }

                _fullscreen = !_fullscreen;
            }
        }
    }

    public class MediaBehavior : Behavior<MediaElement>
    {
        private VideoViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            MediaElement media = AssociatedObject as MediaElement;

            media.MediaOpened += new RoutedEventHandler(media_MediaOpened);
            media.MediaEnded += new RoutedEventHandler(media_MediaEnded);
        }

        void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (!ViewModel.SlideMgr.IsDragging && !ViewModel.SlideMgr.IsClicked)
            {
                ViewModel.SlideMgr.SliderValue = AssociatedObject.Position.TotalSeconds;
                ViewModel.DataMgr.MediaLength = String.Format("{0:00}:{1:00}:{2:00}", AssociatedObject.Position.TotalHours,
                    AssociatedObject.Position.Minutes, AssociatedObject.Position.Seconds);
            }
        }

        void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SlideMgr.PlayTimer != null)
            {
                ViewModel.SlideMgr.PlayTimer.Stop();
                ViewModel.SlideMgr.PlayTimer.Tick -= PlayTimer_Tick;
            }

            ViewModel.DataMgr.CurrentMediaPlaying = null;
            AssociatedObject.Close();

        }

        void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            ViewModel.SlideMgr.SliderValue = 0;

            if (AssociatedObject.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = AssociatedObject.NaturalDuration.TimeSpan;

                (AssociatedObject.FindName("lengthSlider") as Slider).IsEnabled = true;
                ViewModel.SlideMgr.MaximumDuration = ts.TotalSeconds;
                ViewModel.SlideMgr.SliderSmallChange = 1;
                ViewModel.SlideMgr.SliderLargeChange = Math.Min(10, ts.Seconds / 10);

                if (ViewModel.SlideMgr.PlayTimer == null)
                    ViewModel.SlideMgr.PlayTimer = new System.Windows.Threading.DispatcherTimer();

                ViewModel.SlideMgr.PlayTimer.Interval = TimeSpan.FromMilliseconds(1);
                ViewModel.SlideMgr.PlayTimer.Tick += new EventHandler(PlayTimer_Tick);
                ViewModel.SlideMgr.PlayTimer.Start();
            }
            else
            {
                ViewModel.SlideMgr.PlayTimer.Stop();
                ViewModel.SlideMgr.PlayTimer.Tick -= PlayTimer_Tick;
                (AssociatedObject.FindName("lengthSlider") as Slider).IsEnabled = false;
                ViewModel.DataMgr.MediaLength = "00:00:00";
            }
        }
    }

    public class StopBehavior : Behavior<Button>
    {
        private VideoViewModel ViewModel;
        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            AssociatedObject.Click += new System.Windows.RoutedEventHandler(AssociatedObject_Click);
        }

        void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel.DataMgr.CurrentMediaPlaying != null)
            {
                (AssociatedObject.FindName("MediaCtrl") as MediaElement).Close();
                ViewModel.SlideMgr.PlayTimer.Stop();
                ViewModel.SlideMgr.SliderValue = 0;
                ViewModel.DataMgr.MediaLength = "00:00:00";
            }
        }
    }

    public class SliderBehavior : Behavior<Slider>
    {
        private VideoViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            AssociatedObject.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_PreviewMouseLeftButtonDown);
            AssociatedObject.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_PreviewMouseLeftButtonUp);
        }

        void AssociatedObject_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.SlideMgr.IsClicked = false;
            (AssociatedObject.FindName("MediaCtrl") as MediaElement).Play();
        }

        void AssociatedObject_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.SlideMgr.IsClicked = true;
            (AssociatedObject.FindName("MediaCtrl") as MediaElement).Pause();
            (AssociatedObject.FindName("MediaCtrl") as MediaElement).Position = TimeSpan.FromSeconds(AssociatedObject.Value);


        }
    }

    public class PlayBehavior : Behavior<Button>
    {
        private VideoViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as VideoViewModel;
            AssociatedObject.Click += new System.Windows.RoutedEventHandler(AssociatedObject_Click);
        }

        void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel.DataMgr.CurrentMediaPlaying != null)
            {
                if (ViewModel.SlideMgr.PlayTimer != null && (AssociatedObject.FindName("MediaCtrl") as MediaElement).NaturalDuration.HasTimeSpan)
                    ViewModel.SlideMgr.PlayTimer.Start();

                (AssociatedObject.FindName("MediaCtrl") as MediaElement).Play();
            }
        }
    }
}
