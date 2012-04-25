using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWMP.Data;

namespace MyWMP.Attributes
{
	class DisplayAttribute : Attribute
	{
		private List<FileType> _displayFor;
		public List<FileType> DisplayFor
		{
			get { return _displayFor; }
			set { _displayFor = value; }
		}

		public DisplayAttribute()
        {
			_displayFor = new List<FileType>() { FileType.All };
        }

		public DisplayAttribute(FileType typeToDisplay)
		{
			_displayFor = new List<FileType>() { typeToDisplay };
		}

		public DisplayAttribute(params FileType[] typesToDisplay)
		{
			_displayFor = new List<FileType>(typesToDisplay);
		}
	}
}
