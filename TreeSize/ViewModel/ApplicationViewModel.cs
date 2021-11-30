using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TreeSize.Model;

namespace TreeSize.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private BackgroundWorker worker;
        public ObservableCollection<Folder> Folders { get; set; }
        public ObservableCollection<string> Drives { get; set; }
        private string _selectedDrives;
        public string SelectedDrives {
            get => _selectedDrives;
            set
            {
                _selectedDrives = value;

                OnPropertyChanged("SelectedDrives");
            }
        }

        private Command.Command addCommand;
        public Command.Command AddCommand
        {
            get
            {
                return addCommand ??= new Command.Command(o => {
                    Folders.Clear();                    
                    worker.RunWorkerAsync(o.ToString());
                });
            }
        }

        public ApplicationViewModel()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += workerDoWork;
            worker.ProgressChanged += workerProgressChanged;
            worker.RunWorkerCompleted += workerRunWorkerCompleted;
            Drives = GetDrives();
            Folders = new ObservableCollection<Folder>();
        }

        void workerDoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument;

            if (path != null)
            {
                if (IsDirectoryWritable(path))
                {
                    Parallel.ForEach(Directory.EnumerateDirectories(path), dir =>
                    {
                        if (IsDirectoryWritable(dir))
                        {
                                var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir), true);
                                AddFolder(subFolder);
                                AddFile(subFolder);
                                subFolder.InizializeItems();
                                (sender as BackgroundWorker).ReportProgress(100, subFolder);
                        }
                        else
                        {
                                var subFolder = new Folder(dir, dir, false);
                                subFolder.InizializeItems();
                                (sender as BackgroundWorker).ReportProgress(100, subFolder);
                        }                      
                    });
                }
            }

            e.Result = 1;
        }

        void workerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var fold = (Folder)e.UserState;
            if (e.UserState != null)
                Folders.Add(fold);
        }

        void workerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Files received");
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
            Parallel.ForEach(Directory.EnumerateDirectories(folder.FullName), dir =>
            {
                if (IsDirectoryWritable(dir))
                {
                    var subFolder = new Folder(Path.GetFileName(dir), Path.GetFullPath(dir), true);
                    AddFolder(subFolder);
                    AddFile(subFolder);
                    subFolder.InizializeItems();
                    folder.Folders.Add(subFolder);
                }
                else
                {
                    var subFolder = new Folder(dir, dir, false);
                    subFolder.InizializeItems();
                    folder.Folders.Add(subFolder);
                }                
            });
        }

        private void AddFile(Folder folder)
        {
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
