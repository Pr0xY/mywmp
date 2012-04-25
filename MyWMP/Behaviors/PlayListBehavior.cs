using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows.Controls;
using MyWMP.ViewModels;
using System.Windows;
using MyWMP.Data;
using MyWMP.Views;
using System.Windows.Media;
using System.Threading.Tasks;

namespace MyWMP.Behaviors
{
    public class TreeViewItemBehavior : Behavior<TreeViewItem>
    {
        private PlayListViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as PlayListViewModel;
            AssociatedObject.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_PreviewMouseLeftButtonDown);
			
        }

        void AssociatedObject_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch ((string)AssociatedObject.Tag)
            {
                case "Video":
                    ViewModel.MediaMgr.CurrentMediaListSelected = ViewModel.MediaMgr.MyVideos;					
                    break;
                case "Music":
                    ViewModel.MediaMgr.CurrentMediaListSelected = ViewModel.MediaMgr.MyMusic;
                    break;
                case "Pic":
                    ViewModel.MediaMgr.CurrentMediaListSelected = ViewModel.MediaMgr.MyPictures;
                    break;

            }
        }
    }

    public class PlayListItemBehavior : Behavior<TreeViewItem>
    {
        private PlayListViewModel ViewModel;

        protected override void OnAttached()
        {
            
            ViewModel = AssociatedObject.DataContext as PlayListViewModel;
            AssociatedObject.Selected += new RoutedEventHandler(AssociatedObject_Selected);
			AssociatedObject.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(AssociatedObject_PreviewKeyDown);			
        }

		void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			throw new NotImplementedException();
		}

        void AssociatedObject_Selected(object sender, RoutedEventArgs e)
        {
            if ((e.OriginalSource as TreeViewItem).DataContext is PlayListViewModel)
                return;

			Dispatcher.BeginInvoke(new Action(delegate()
			{
				ViewModel.MediaMgr.CurrentMediaListSelected = ((e.OriginalSource as TreeViewItem).DataContext as Playlist).Files;
			}));
        }

		void AssociatedObject_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			switch (e.Key)
			{
				case System.Windows.Input.Key.Delete:
					MessageBoxResult result = MessageBox.Show("Etes vous sur de vouloir supprimer cette liste de lecture ?", "Suppression d'une liste de lecture", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.Yes)
					{
						Playlist playlistToDelete = (e.OriginalSource as TreeViewItem).DataContext as Playlist;
						ViewModel.MediaMgr.Playlists.Remove(playlistToDelete);
						ViewModel.MediaMgr.CurrentMediaListSelected = null;
						ViewModel.MediaMgr.DeletePlayList(playlistToDelete);
					}

					break;
				default:
					break;
			}
		}
    }

    public class DataGridSelectionBehavior : Behavior<DataGrid>
    {
        private PlayListViewModel ViewModel;

        protected override void OnAttached()
        {
            ViewModel = AssociatedObject.DataContext as PlayListViewModel;
            AssociatedObject.PreviewMouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_PreviewMouseDoubleClick);
			AssociatedObject.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(AssociatedObject_AutoGeneratingColumn);
        }

		void AssociatedObject_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			
		}

        void AssociatedObject_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			if (AssociatedObject.SelectedItem != null)
			{
				((Application.Current.MainWindow.FindName("VideoCtrl") as VideoView).DataContext as VideoViewModel).DataMgr.CurrentMediaPlaying = (AssociatedObject.SelectedItem as Media).Path;
				(Application.Current.MainWindow.DataContext as MainWindowViewModel).SelectedIndex = 1;
				((Application.Current.MainWindow.FindName("VideoCtrl") as VideoView).FindName("MediaCtrl") as MediaElement).Play();
			}
        }
    }
}
