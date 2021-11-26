using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Controls;
using TreeSize.Model;

namespace TreeSize.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        //private List<string> _errors;
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
            //IsDirectoryWritable(@"C:\Temp");
            //IsDirectoryWritable(@"‪C:\Users\Default\Cookies");
            // _errors = new List<string>();
            //Folders = ChooseDisk(@"C:\Users\Default\Cookies");
            Folders = ChooseDisk(@"C:\Program Files (x86)\Microsoft\EdgeUpdate");
        }

        private ObservableCollection<string> GetDrives()
        {
            var localDrives = new ObservableCollection<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
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
                if (IsDirectoryWritable(path))
                {
                    Parallel.ForEach(Directory.EnumerateDirectories(path), dir =>
                    {
                        if (IsDirectoryWritable(dir))
                        {
                            var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir));
                            AddFolder(subFolder);
                            AddFile(subFolder);
                            subFolder.InizializeItems();
                            fold.Add(subFolder);
                        }
                        else
                        {
                            var subFolder = new Folder(dir, dir);
                            subFolder.InizializeItems();
                            fold.Add(subFolder);
                        }
                    });
                }

                //if (IsDirectoryWritable(path))
                //{
                //    var directories = Directory.EnumerateDirectories(path);

                //    foreach (var dir in directories)
                //    {
                //        if (IsDirectoryWritable(dir))
                //        {
                //            var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir));
                //            AddFolder(subFolder);
                //            AddFile(subFolder);
                //            subFolder.InizializeItems();
                //            fold.Add(subFolder);
                //        }
                //        else
                //        {
                //            var subFolder = new Folder(dir, dir);
                //            subFolder.InizializeItems();
                //            fold.Add(subFolder);
                //        }
                //    }
                //}
            }

            return fold;
        }

        private bool IsDirectoryWritable(string dirPath)
        {
            try
            {
                var directories = Directory.EnumerateDirectories(dirPath);

                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }
        }

        private void AddFolder(Folder folder)
        {
        //    var directories = Directory.EnumerateDirectories(folder.FullName);

        //    foreach (var dir in directories)
        //    {
        //        if (IsDirectoryWritable(dir))
        //        {
        //            var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir));
        //            AddFolder(subFolder);
        //            AddFile(subFolder);
        //            subFolder.InizializeItems();
        //            folder.Folders.Add(subFolder);
        //        }
        //        else
        //        {
        //            var subFolder = new Folder(dir, dir);
        //            subFolder.InizializeItems();
        //            folder.Folders.Add(subFolder);
        //        }
        //    }

            Parallel.ForEach(Directory.EnumerateDirectories(folder.FullName), dir => //полные имена каталогов
            {
                if (IsDirectoryWritable(dir))
                {
                    var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir));
                    AddFolder(subFolder);
                    AddFile(subFolder);
                    subFolder.InizializeItems();
                    folder.Folders.Add(subFolder);
                }
                else
                {
                    var subFolder = new Folder(dir, dir);
                    subFolder.InizializeItems();
                    folder.Folders.Add(subFolder);
                }
            });
        }

        private void AddFile(Folder folder)
        {
            //var directories = Directory.EnumerateFiles(folder.FullName);
            //foreach(var fileWay in directories)
            //{
            //    var file = new Model.File(Path.GetFileName(fileWay), Path.GetFullPath(fileWay));
            //    folder.Files.Add(file);
            //}

            Parallel.ForEach(Directory.EnumerateFiles(folder.FullName), fileWay =>
            {
                var file = new Model.File(Path.GetFileName(fileWay), Path.GetFullPath(fileWay));
                folder.Files.Add(file);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
