using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using TreeSize.Model;

namespace TreeSize.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Folder> Folders { get; set; }
        public ObservableCollection<string> Drives { get; set; }
        private string _selectedDrives;
        public string SelectedDrives {
            get => _selectedDrives;
            set
            {
                _selectedDrives = value;
                //Folders.Clear();
                //var tree = ChooseDisk(_selectedDrives);

                //foreach (var item in tree)
                //{
                //    Folders.Add(item);
                //}

                OnPropertyChanged("SelectedDrives");
            }
        }

        private Command.Command addCommand;
        public Command.Command AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new Command.Command(o => {
                      Folders.Clear();
                      var tree = ChooseDisk(o.ToString());

                      foreach (var item in tree)
                      {
                          Folders.Add(item);
                      }
                  }));
            }
        }

        public ApplicationViewModel()
        {
            Drives = GetDrives();
            Drives.Add("C:\\FoxMined");
            Drives.Add("C:\\UNN");
            Folders = new ObservableCollection<Folder>();
        }

        private ObservableCollection<string> GetDrives()
        {
            var localDrives = new ObservableCollection<string>();
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            foreach (System.IO.DriveInfo drive in drives)
            {
                localDrives.Add(drive.Name);
            }

            return localDrives;
        }

        private ObservableCollection<Folder> ChooseDisk(string path)
        {
            var fold = new ObservableCollection<Folder>();

            if (path != null)
            {
                Parallel.ForEach(System.IO.Directory.EnumerateDirectories(path), dir =>
               {
                   var subFolder = new Folder(System.IO.Path.GetFileName(dir), System.IO.Path.GetFullPath(dir));
                   AddFolder(subFolder);
                   AddFile(subFolder);
                   subFolder.InizializeItems();
                   fold.Add(subFolder);
               });
            }

            return fold;
        }

        private void AddFolder(Folder folder)
        {
            //try
            //{
                Parallel.ForEach(System.IO.Directory.EnumerateDirectories(folder.FullName), dir =>
               {
                   var subFolder = new Folder(System.IO.Path.GetFileName(dir), System.IO.Path.GetFullPath(dir));
                   AddFolder(subFolder);
                   AddFile(subFolder);
                   subFolder.InizializeItems();
                   folder.Folders.Add(subFolder);
               });
            //}
            //catch
            //{

            //}
        }

        private void AddFile(Folder folder)
        {
            //try
            //{
                Parallel.ForEach(System.IO.Directory.EnumerateFiles(folder.FullName), fileWay =>
                {
                    var file = new File(System.IO.Path.GetFileName(fileWay), System.IO.Path.GetFullPath(fileWay));
                    folder.Files.Add(file);
                });
            //}
            //catch
            //{

            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
