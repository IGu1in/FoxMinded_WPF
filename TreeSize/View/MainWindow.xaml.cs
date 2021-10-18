using System.Windows;
using TreeSize.ViewModel;

namespace TreeSize
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}
