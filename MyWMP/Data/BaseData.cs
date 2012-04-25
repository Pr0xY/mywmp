using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using MyWMP.Attributes;
using System.Reflection;

namespace MyWMP.Data
{
    [Serializable]
    public abstract class BaseData : INotifyPropertyChanged, ISerializable
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Serialization

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Error : SerializationInfo is null !");

            var serializableProps = (from el in this.GetType().GetProperties()
                                     where el.GetCustomAttributes(typeof(SerializeAttribute), true).Count() > 0
                                     select el);

            foreach (PropertyInfo prop in serializableProps)
                info.AddValue(prop.Name, prop.GetValue(this, null));
        }

        #endregion

        public BaseData() { }
    }
}
