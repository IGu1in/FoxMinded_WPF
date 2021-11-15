using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeSize.Model
{
    public class File : FileStructure, INotifyPropertyChanged
    {
        private string _size;
        private string _info;
        private long _totalSize = 0;
        public new string Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged("Info");
            }
        }
  
        public File(string name, string fullName) : base(name, fullName)
        {
            _size = FormatSize();
            Items = new ObservableCollection<Folder>();
            _info = Name + " Size: " + _size;
        }

        private string FormatSize()
        {
            GetFileSize(FullName);

            if (_totalSize > Math.Pow(1024, 3))
            {
                string size = Math.Round(_totalSize / Math.Pow(1024, 3), 3).ToString() + " Гб.";

                return size;
            }
            else
            {
                if (_totalSize > Math.Pow(1024, 2))
                {
                    string size = Math.Round(_totalSize / Math.Pow(1024, 2), 3).ToString() + " Мб.";

                    return size;
                }
                else
                {
                    if (_totalSize > Math.Pow(1024, 1))
                    {
                        string size = Math.Round(_totalSize / Math.Pow(1024, 1), 3).ToString() + " Кб.";

                        return size;
                    }
                    else
                    {
                        string size = (_totalSize).ToString() + " Байт";

                        return size;
                    }
                }
            }
        }

        private void GetFileSize(string path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            _totalSize += fi.Length;
        }
    }
}
