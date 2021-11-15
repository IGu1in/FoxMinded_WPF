using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TreeSize.Model
{
    public class Folder : FileStructure, INotifyPropertyChanged
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

        public ObservableCollection<File> Files { get; set; }
        public ObservableCollection<Folder> Folders { get; set; }
        public new ObservableCollection<object> Items { get; set; }
        private IEnumerable<object> _items
        {
            get
            {
                foreach (var folder in Folders)
                    yield return folder;
                foreach (var file in Files)
                    yield return file;
            }
        }
        public Folder(string name, string fullName) : base(name, fullName)
        {
            Folders = new ObservableCollection<Folder>();
            _size = FormatSize();
            Files = new ObservableCollection<File>();
            _info = Name + " Size: " + _size;
        }

        public void InizializeItems()
        {
            Items = new ObservableCollection<object>(_items);
        }

        public string GetFilesInfo()
        {
            string info = "";

            foreach (var file in Files)
            {
                info += file.Name + file.Size + "; ";
            }

            return info;
        }

        public IEnumerable<Folder> GetEnumerator(string path)
        {
            foreach (var dir in Folders)
            {
                var way = dir.FullName;
                GetEnumerator(way);

                yield return dir;
            }
        }

        private string FormatSize()
        {
            GetTotalSize(FullName);

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

        private void GetTotalSize(string directory)
        {
            //try
            //{
                foreach (var file in System.IO.Directory.EnumerateFiles(directory))
                {
                    GetFileSize(file);
                }
            //}
            //catch
            //{
            //    _size = "not enough rights";
            //}

            //try
            //{
                foreach (var dir in System.IO.Directory.EnumerateDirectories(directory))
                {
                    GetTotalSize(dir);
                }
            //}
            //catch
            //{
            //    _size = "not enough rights";
            //}
        }

        private void GetFileSize(string path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            _totalSize += fi.Length;
        }
    }
}
