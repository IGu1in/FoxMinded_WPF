using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeSize.Model
{
    public class File : INotifyPropertyChanged
    {
        private string _name;
        private string _fullName;
        private string _size;
        private long _totalSize = 0;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged("Size");
            }
        }

        public File(string name, string fullName)
        {
            _name = name;
            _fullName = fullName;
            _size = FormatSize();
        }

        private string FormatSize()
        {
            GetFileSize(FullName);

            if (_totalSize > Math.Pow(1024, 3))
            {
                string size = Math.Round(_totalSize / Math.Pow(1024, 1), 3).ToString() + " Гб.";

                return size;
            }
            else
            {
                if (_totalSize > Math.Pow(1024, 2))
                {
                    string size = Math.Round(_totalSize / Math.Pow(1024, 1), 3).ToString() + " Мб.";

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
