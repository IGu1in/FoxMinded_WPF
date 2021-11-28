using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeSize.Model
{
    public class FileStructure : INotifyPropertyChanged
    {
        private string _name;
        private string _fullName;
        private string _size;
        private string _info;
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
        public string Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged("Info");
            }
        }

        public ObservableCollection<Folder> Items { get; set; }

        public FileStructure(string name, string fullName)
        {
            _name = name;
            _fullName = fullName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
