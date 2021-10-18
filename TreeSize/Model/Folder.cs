using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeSize.Model
{
    public class Folder : INotifyPropertyChanged
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

        public ObservableCollection<File> Files { get; set; }
        public IEnumerable<Folder> Folders { get; set; }

        public Folder(string name, string fullName, Folder[] children)
        {
            _name = name;
            _fullName = fullName;
            Folders = children;
            _size = FormatSize();
            Files = new ObservableCollection<File>();
        }

        private string FormatSize()
        {
            GetTotalSize(FullName);

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

        private void GetTotalSize(string directory)
        {
            foreach (var file in System.IO.Directory.EnumerateFiles(directory))
            {
                GetFileSize(file);
            }

            foreach (var dir in System.IO.Directory.EnumerateDirectories(directory))
            {
                GetTotalSize(dir);
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
