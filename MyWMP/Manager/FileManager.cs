using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWMP.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace MyWMP
{
    public class FileManager : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        //public void NotifyCollectionChanged(String name)
        //{
        //    if (CollectionChanged != null)
        //        CollectionChanged(this, new NotifyCollectionChangedEventArgs(prop));
        //}

        #endregion

        private const string musicSearchPattern = ".mp3";
        private const string videoSearchPattern = ".avi;.mpg;.wmv";
        private const string picsSearchPattern = ".jpg;.png;.bmp";

        private DirectoryInfo myMusicPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        private DirectoryInfo myVideosPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        private DirectoryInfo myPicturesPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
		private DirectoryInfo publicPicturesPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
		private DirectoryInfo publicVideosPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
		private DirectoryInfo publicMusicPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
        private DirectoryInfo myPlayListPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MyWindowsMediaPlayer\MyPlayLists");

        private ObservableCollection<Media> _commonLibrary = new ObservableCollection<Media>();
        public ObservableCollection<Media> CommonLibrary
        {
            get { return _commonLibrary; }
            private set { value = _commonLibrary; }
        }

		public ObservableCollection<Media> MyVideos
        {
            get { return GetFiles(FileType.Video); }
        }

		public ObservableCollection<Media> MyMusic
        {
            get { return GetFiles(FileType.Son); }
        }

		public ObservableCollection<Media> MyPictures
        {
            get { return GetFiles(FileType.Image); }
        }

		private ObservableCollection<Media> _currentMediaListSelected;
		public ObservableCollection<Media> CurrentMediaListSelected
        {
            get { return _currentMediaListSelected; }
            set
            {
                _currentMediaListSelected = value;
                NotifyPropertyChanged("CurrentMediaListSelected");
            }
        }

        private ObservableCollection<Playlist> _playlists = new ObservableCollection<Playlist>();
        public ObservableCollection<Playlist> Playlists
        {
            get { return _playlists; }
            private set
            {
                value = _playlists;
                NotifyPropertyChanged("Playlists");

            }
        }

        #region GetFiles

        //quand tu veux loader tout les fichier d'un certain type;
		public ObservableCollection<Media> GetFiles(FileType fileType)
        {
            var res = from el in CommonLibrary.AsParallel() where el.Type == fileType select el;
			return new ObservableCollection<Media>(res.ToList());
        }
        //public ObservableCollection<Media> GetFiles(Type T)
        //{
        //    var res = from el in CommonLibrary where el.GetType() == T select el;
        //    return new ObservableCollection<Media>(res.ToList());
        //}
        //public ObservableCollection<Media> GetFiles(Playlist playlist)
        //{
        //    var res = from el in CommonLibrary where el.GetType() == typeof(Playable) && (el as Playable).Playlists.First(item => item == playlist) != null select el;
        //    return new ObservableCollection<Media>(res.ToList());
        //}

        #endregion

        public void DeletePlayList(Playlist p)
        {
            var playListToDelete = (from el in Playlists
                                    where el.Name == p.Name
                                    select el);

            if (playListToDelete.Count() == 0)
                return;

            Playlists.Remove(p);

            if (File.Exists(myPlayListPath + "\\" + p.Name + ".xml"))
                File.Delete(myPlayListPath + "\\" + p.Name + ".xml");
        }

        public void AddPlayList(string playListName)
        {
            XDocument newPlaylist = new XDocument(new XElement("PlayList"));

            if (File.Exists(myPlayListPath + "\\" + playListName + ".xml"))
            {
                MessageBoxResult res = MessageBox.Show("This playlist already exists. Do you want to overwrite it ?", "Playlist already existant", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    newPlaylist.Root.Add(new XElement("Name", playListName));
                    newPlaylist.Root.Add(new XElement("Files"));
                    newPlaylist.Save(myPlayListPath + "\\" + playListName + ".xml");
                    Playlists.Add(new Playlist()
                    {
                        Name = playListName,
                        Files = new ObservableCollection<Media>()
                    });
                }
            }
            else
            {
                newPlaylist.Root.Add(new XElement("Name", playListName));
                newPlaylist.Root.Add(new XElement("Files"));
                newPlaylist.Save(myPlayListPath + "\\" + playListName + ".xml");

                Playlists.Add(new Playlist()
                {
                    Name = playListName,
                    Files = new ObservableCollection<Media>()
                });
            }
        }

        public FileManager()
        {			
            LoadCommonLibraryContent();
            CreateDummyPlaylist();
            LoadCustomPlayLists();

            CurrentMediaListSelected = MyVideos;
        }

        private void LoadCommonLibraryContent()
        {
			Parallel.ForEach(videoSearchPattern.Split(';'), searchPatern =>
			{
				GetMedias(publicVideosPath, searchPatern, FileType.Video);
				GetMedias(myVideosPath, searchPatern, FileType.Video);
			});

			Parallel.ForEach(musicSearchPattern.Split(';'), searchPatern =>
			{
				GetMedias(publicMusicPath, searchPatern, FileType.Son);
				GetMedias(myMusicPath, searchPatern, FileType.Son);
			});

			Parallel.ForEach(picsSearchPattern.Split(';'), searchPatern =>
			{
				GetMedias(publicPicturesPath, searchPatern, FileType.Image);
				GetMedias(myPicturesPath, searchPatern, FileType.Image);
			});
        }

        private void CreateDummyPlaylist()
        {
            if (!myPlayListPath.Exists)
                myPlayListPath.Create();

                XDocument xmlPlayList = new XDocument(new XElement("PlayList",
                            new XElement("Name", "Dummy"),
                            new XElement("Files", (from el in CommonLibrary
                                                   select new XElement("Media",
                                                       new XElement("Title", el.Title),
                                                       new XElement("Length", el.Duree),
                                                       new XElement("Path", el.Path),
                                                       new XElement("Type", el.Type.ToString()))
                                                       ))));
                xmlPlayList.Save(myPlayListPath + "\\DummyPlaylist.xml");
        }

        private void GetMedias(DirectoryInfo dir, string searchPatern, FileType mediaType)
        {
            foreach (FileSystemInfo media in dir.GetFileSystemInfos())
            {
                if (media is DirectoryInfo)
                    GetMedias(media as DirectoryInfo, searchPatern, mediaType);
                else
                {
                    if (media.Extension == searchPatern)
                    {
                        switch (mediaType)
                        {
                            case FileType.Video:
                                AddVideo(media as FileInfo);
                                break;
                            case FileType.Son:
                                AddMusic(media as FileInfo);
                                break;

                            case FileType.Image:
                                AddImage(media as FileInfo);
                                break;
                        }
                    }
                }
            }
        }

        private void AddMusic(FileInfo music)
        {
            TagLib.File musicFile = TagLib.File.Create(music.FullName);

            CommonLibrary.Add(new Media(musicFile.Tag.Title, String.Format("{0:00}:{1:00}:{2:00}", musicFile.Properties.Duration.TotalHours, musicFile.Properties.Duration.TotalMinutes, musicFile.Properties.Duration.TotalSeconds), music.FullName, FileType.Son)
            {
                Filename = music.Name,
                Annee = (int)musicFile.Tag.Year,
                Commentaire = musicFile.Tag.Comment,
                CreationDate = music.CreationTime,
                Genre = musicFile.Tag.FirstGenre,
                ModificationDate = music.LastWriteTime,
                Album = musicFile.Tag.Album,
                Artist = musicFile.Tag.FirstAlbumArtist,
            });
        }

        private void AddImage(FileInfo pic)
        {
            TagLib.File picFile = TagLib.File.Create(pic.FullName);

            CommonLibrary.Add(new Media(pic.Name, pic.FullName)
            {
                Filename = pic.Name,
                Commentaire = picFile.Tag.Comment,
                CreationDate = pic.CreationTime,
                ModificationDate = pic.LastWriteTime
            });
        }

        private void AddVideo(FileInfo video)
        {
            TagLib.File videoFile = TagLib.File.Create(video.FullName);

            CommonLibrary.Add(new Media(videoFile.Tag.Title, String.Format("{0:00}:{1:00}:{2:00}", videoFile.Properties.Duration.TotalHours, videoFile.Properties.Duration.TotalMinutes, videoFile.Properties.Duration.TotalSeconds), video.FullName, FileType.Video)
            {
                Filename = video.Name,
                Annee = (int)videoFile.Tag.Year,
                Commentaire = videoFile.Tag.Comment,
                CreationDate = video.CreationTime,
                Genre = videoFile.Tag.FirstGenre,
                ModificationDate = video.LastWriteTime
            });
        }

        private void LoadCustomPlayLists()
        {
            if (!myPlayListPath.Exists)
                myPlayListPath.Create();

			Parallel.ForEach(myPlayListPath.GetFiles("*.xml", SearchOption.TopDirectoryOnly), playList =>
			{
				XDocument xmlPlayList = XDocument.Load(playList.Open(FileMode.Open));

				if (xmlPlayList.Root.Name == "PlayList")
				{
					try
					{
						if (xmlPlayList.Root.Element("Name") != null && xmlPlayList.Root.Element("Files") != null)
						{
							Playlists.Add(new Playlist()
							{
								Name = xmlPlayList.Root.Element("Name").Value,
								Files = new ObservableCollection<Media>((from el in xmlPlayList.Root.Element("Files").Elements()
																		 select new Media(el.Element("Title").Value,
																			 el.Element("Length").Value,
																			 el.Element("Path").Value,
																			 (FileType)Enum.Parse(typeof(FileType), el.Element("Type").Value))).ToList())
							});
						}
					}
					catch
					{
						MessageBox.Show("This file is not a valid MyWindowsMediaPlayer playlist file !", "Error loading playlist", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			});
        }

        public void SaveCustomPlayLists()
        {
            if (!myPlayListPath.Exists)
                myPlayListPath.Create();

			Parallel.ForEach(Playlists, playList =>
			{
				XDocument xmlPlayList = new XDocument(new XElement("PlayList",
							new XElement("Name", playList.Name),
							new XElement("Files", (from el in playList.Files
												   select new XElement("Media",
													   new XElement("Title", el.Title),
													   new XElement("Length", el.Duree),
													   new XElement("Path", el.Path),
													   new XElement("Type", el.Type.ToString()))
													   ))));
				xmlPlayList.Save(myPlayListPath + "\\" + playList.Name + ".xml");
			});
        }
    }
}
