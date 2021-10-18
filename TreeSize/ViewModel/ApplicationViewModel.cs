using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TreeSize.Model;

namespace TreeSize.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public IEnumerable<Folder> Folders { get; set; }
        public ObservableCollection<File> Files { get; set; }
        public ObservableCollection<object> Items { get; set; }
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

        public ApplicationViewModel()
        {
            Files = new ObservableCollection<File>();
            Folders = BuildTree("C:\\FoxMined");
            Items = new ObservableCollection<object>(_items);
        }

        private IEnumerable<Folder> BuildTree(string path)
        {
            foreach (var dir in System.IO.Directory.EnumerateDirectories(path))
            {
                var folder = new Folder(System.IO.Path.GetFileName(dir), System.IO.Path.GetFullPath(dir), BuildTree(dir).ToArray());
                FindFile(folder);

                yield return folder;
            }
        }

        private void FindFile(Folder folder)
        {
            foreach (var file in System.IO.Directory.EnumerateFiles(folder.FullName))
            {
                folder.Files.Add(new File(System.IO.Path.GetFileName(file), System.IO.Path.GetFullPath(file)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
