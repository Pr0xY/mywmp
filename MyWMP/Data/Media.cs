using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Reflection;
using MyWMP.Attributes;
using System.IO;
using System.Collections.ObjectModel;

namespace MyWMP.Data
{
    public enum FileType
    {
        Video,
        Son,
        Image,
        Enregistrement,
		All,
		None
    }

    [Serializable]
    public class Media : BaseData
    {
        #region Properties

        private string _title;		
		[Display]
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        private string _artist;
		[Display(FileType.Son)]
        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        private string _album;
		[Display(FileType.Son)]
        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        private string _duree;
		[Display(FileType.Son, FileType.Video)]
        public string Duree
        {
            get { return _duree; }
            set { _duree = value; NotifyPropertyChanged("Duree"); }
        }

        private int _annee;
		[Display(FileType.Son)]
        public int Annee
        {
            get { return _annee; }
            set { _annee = value; NotifyPropertyChanged("Annee"); }
        }

        private string _genre;
		[Display(FileType.Son, FileType.Video)]
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; NotifyPropertyChanged("Genre"); }
        }

		//private double _width;
		//public double Width
		//{
		//    get { return _width; }
		//    set { _width = value; NotifyPropertyChanged("Width"); }
		//}

		//private double _height;
		//public double Height
		//{
		//    get { return _height; }
		//    set { _height = value; NotifyPropertyChanged("Height"); }
		//}

        private string _filename;
		[Display(FileType.None)]
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; NotifyPropertyChanged("Filename"); }
        }

        private string _path;
		[Display(FileType.None)]
		public string Path
        {
            get { return _path; }
            set { _path = value; NotifyPropertyChanged("Path"); }
        }


        private DateTime _creationDate;
		[Display(FileType.None)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; NotifyPropertyChanged("CreationDate"); }
        }

        private DateTime _modificationDate;
		[Display(FileType.None)]
        public DateTime ModificationDate
        {
            get { return _modificationDate; }
            set { _modificationDate = value; NotifyPropertyChanged("ModificationDate"); }
        }

        private string _commentaire;
        [Serialize]
		[Display(FileType.None)]
        public string Commentaire
        {
            get { return _commentaire; }
            set { _commentaire = value; NotifyPropertyChanged("Commentaire"); }
        }


        private FileType _type;
		[Display(FileType.None)]
		public FileType Type
        {
            get { return _type; }
            set { _type = value; NotifyPropertyChanged("Type"); }
        }

        #endregion

        #region Constructors

        public Media(string title, string length, string path, FileType fileType)
        {
            Title = title;
            Duree = length;
            Path = path;
            Type = fileType;
        }

        public Media(string title, string path)
        {
            Title = title;
            Path = path;
            Type = FileType.Image;
        }

        public Media(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Error : SerializationInfo is null !");

            var serializableProps = (from el in this.GetType().GetProperties()
                                     where el.GetCustomAttributes(typeof(SerializeAttribute), true).Count() > 0
                                     select el);

            foreach (PropertyInfo prop in serializableProps)
                prop.SetValue(this, info.GetValue(prop.Name, prop.PropertyType), null);
        }

        #endregion
    
    }
}
